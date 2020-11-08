using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum Inputs
{
    Idle,
    Horizontal,
    Jump,
    Sliding
}
public class PlayerMovement : MonoBehaviour
{
    Inputs m_inputChoices;
    Rigidbody2D m_rb2d;
    [SerializeField]
    float m_speed;
    [SerializeField]
    float m_speedBoost;
    [SerializeField]
    float m_jumpForce;
    [SerializeField]
    float m_groundSpeedMultiplier;
    [SerializeField]
    float m_airSpeedMultiplier;
    GroundCheck m_groundCheck;
    [SerializeField]
    Vector3 m_jumpStretchSize;
    [SerializeField]
    Vector3 m_slideStretchSize;
    float m_horizontal;
    public Transform m_frontCheck;
    bool m_grabbedWall;
    bool m_wallSliding;
    bool m_isJumping;
    bool m_isSliding;
    [SerializeField]
    float m_wallSlidingSpeed;
    LayerMask m_wallMask;
    [SerializeField]
    float m_checkRadius;
    bool m_wallJumping;
    [SerializeField]
    float m_xWallForce;
    [SerializeField]
    float m_yWallForce;
    [SerializeField]
    float m_wallJumpTime;
    [SerializeField]
    float m_maxVelo;
    ParticleSystem m_jumpParticles;
    // Start is called before the first frame update
    GhostReplay m_replay;
    Vector2 m_oldVelo;


    void Start()
    {
        GameObject rope = transform.GetChild(5).gameObject;
        m_rb2d = GetComponent<Rigidbody2D>();
        m_groundCheck = GetComponent<GroundCheck>();
        m_wallMask = LayerMask.GetMask("Wall");
        m_replay = GameObject.Find("Ghost").GetComponent<GhostReplay>();
        m_jumpParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
        m_grappleLine = transform.GetChild(4).GetComponent<LineRenderer>();
        rope.gameObject.SetActive(false);
        rope.gameObject.SetActive(true);
        Invoke("ActivateRope", 1);
    }
    void ActivateRope()
    {

    }
    // Update is called once per frame
    void Update()
    {
        WallJumping();
        //    m_replay.AddPositions(transform.position);
        //   m_replay.AddHorizontalValues(m_horizontal);
        if (m_groundCheck.isGrounded() && !m_isJumping && !m_isSliding)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        MoveInput();
        if (m_horizontal < 0)
        {
            m_frontCheck.position = new Vector2(transform.position.x - 0.1f, transform.position.y);
        }
        else
        {
            m_frontCheck.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
        }
        SquishEffects();
        RunGrapple();
    }
    private void FixedUpdate()
    {
        WallGrab();
    }
    void SquishEffects()
    {
        if (!m_groundCheck.isGrounded())
        {
            SquishEffectJump();
        }
        if (m_isSliding)
        {
            SlideSquishDown();
        }
    }
    void MoveInput()
    {
        m_horizontal = Input.GetAxis("Horizontal");
        if (m_horizontal != 0)
        {
            m_inputChoices = Inputs.Horizontal;
        }
        if (Input.GetKeyDown(KeyCode.Space) && m_groundCheck.isGrounded() && !m_swinging)
        {
            m_inputChoices = Inputs.Jump;
            m_groundCheck.setIsGround(false);
            m_isJumping = true;
            m_oldVelo = m_rb2d.velocity;
            MakeDust();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !m_isSliding && m_groundCheck.isGrounded())
        {
            m_inputChoices = Inputs.Sliding;
            m_isSliding = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && m_isSliding && m_groundCheck.isGrounded())
        {
            m_isSliding = false;
            m_speed = m_speed / m_speedBoost;
        }
        MoveChoice(m_inputChoices);
    }
    void MoveChoice(Inputs _inputChoice)
    {
        switch (m_inputChoices)
        {
            case Inputs.Horizontal:
                if (m_groundCheck.isGrounded())
                    Move(m_horizontal, 0, m_speed, m_groundSpeedMultiplier);
                else
                    Move(m_horizontal, 0, m_speed, m_airSpeedMultiplier);
                break;
            case Inputs.Jump:
                Move(0, m_jumpForce);
                break;
            case Inputs.Sliding:
                m_speed *= m_speedBoost;
                break;
            default:
                break;
        }
        m_inputChoices = Inputs.Idle;
    }
    void Move(float _xValue, float _yValue, float _speed = 1, float _multiplier = 1)
    {
        m_rb2d.AddForce(new Vector2(_xValue, _yValue) * _speed * _multiplier);
        if (m_groundCheck.isGrounded() || m_isJumping)
            m_rb2d.velocity = Vector2.ClampMagnitude(m_rb2d.velocity, m_maxVelo);
        if (m_isJumping)
            m_isJumping = false;
    }
    void SquishEffectJump()
    {
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, m_jumpStretchSize.y, transform.localScale.y), transform.localScale.z);
    }
    void SlideSquishDown()
    {
        transform.localScale = new Vector3(1, 0.8f, 1);
    }
    void WallGrab()
    {
        m_grabbedWall = Physics2D.OverlapCircle(m_frontCheck.position, m_checkRadius, m_wallMask);

        if (m_grabbedWall && !m_groundCheck.isGrounded() && m_horizontal != 0)
            m_wallSliding = true;
        else
            m_wallSliding = false;
        if (m_wallSliding)
            m_rb2d.velocity = new Vector2(m_rb2d.velocity.x, Mathf.Clamp(m_rb2d.velocity.y, -m_wallSlidingSpeed, float.MaxValue));

        if (m_wallJumping)
        {
            if (m_oldVelo.x == 0)
                m_rb2d.velocity = new Vector2(m_xWallForce * m_horizontal, m_yWallForce);
            else
            {
                if (m_oldVelo.x < 0)
                    m_rb2d.velocity = new Vector2(-m_oldVelo.x * m_horizontal, m_yWallForce);
                else
                    m_rb2d.velocity = new Vector2(m_oldVelo.x * m_horizontal, m_yWallForce);
            }
        }
    }
    void WallJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_wallSliding)
        {
            m_wallJumping = true;
            Invoke("SetWallJumpingFalse", m_wallJumpTime);
            MakeDust();

        }
    }
    void SetWallJumpingFalse()
    {
        m_wallJumping = false;
    }

    void MakeDust()
    {
        m_jumpParticles.Play();
    }
    #region Grapple Code
    [SerializeField]
    float m_grappleRange;
    bool m_grappleAttached;
    bool m_swinging;
    LineRenderer m_grappleLine;

    public DistanceJoint2D RopeJoint;
    List<Vector2> m_ropePositions = new List<Vector2>();
    bool m_distanceSet;
    public Rigidbody2D RopeAnchor;
    void RunGrapple()
    {
        if (Input.GetKey(KeyCode.Q))
            GrappleToPoint();
        if (Input.GetKeyUp(KeyCode.Q))
        {
            RopeReset();
            m_swinging = false;
        }
        ResetRopePositions();
    }
    public void RopeReset()
    {
        RopeJoint.enabled = false;
        m_grappleAttached = false;
        m_grappleLine.positionCount = 2;
        m_grappleLine.SetPosition(0, transform.position);
        m_grappleLine.SetPosition(1, transform.position);
        m_ropePositions.Clear();
    }
    void GrappleToPoint()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, m_grappleRange);
        if (hit.collider != null)
        {
            if (!hit.collider.tag.Contains("Enemy") || !hit.collider.tag.Contains("Bullet") || !hit.collider.tag.Contains("Player"))
            {
                m_grappleAttached = true;
                if (m_ropePositions.Count < 1)
                {
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
                    m_ropePositions.Add(hit.point);
                    RopeJoint.distance = Vector2.Distance(transform.position, hit.point);
                    RopeJoint.enabled = true;
                    m_swinging = true;
                }
            }
        }
      
    }
    void ResetRopePositions()
    {
        if (!m_grappleAttached)
        {
            return;
        }
        m_grappleLine.positionCount = m_ropePositions.Count + 1;
        for (int i = m_grappleLine.positionCount - 1; i >= 0; --i)
        {
            if (i != m_grappleLine.positionCount - 1)
            {
                m_grappleLine.SetPosition(i, m_ropePositions[i]);
                if (i == m_ropePositions.Count - 1 || m_ropePositions.Count == 1)
                    SetVector2RopePos(m_ropePositions);
                else if (i - 1 == m_ropePositions.IndexOf(m_ropePositions.Last()))
                    SetVector2RopePos(m_ropePositions,"Last");
            }
            else
                m_grappleLine.SetPosition(i, transform.position);
        }
    }
    void SetVector2RopePos(List<Vector2> _vec,string _state = "")
    {
        Vector2 ropePosition;
        if (_state == "Last")
            ropePosition = m_ropePositions.Last();
        else
            ropePosition = _vec[_vec.Count - 1];
        RopeAnchor.transform.position = ropePosition;
        if (!m_distanceSet)
        {
            RopeJoint.distance = Vector2.Distance(transform.position, ropePosition);
            m_distanceSet = true;
        }
    }
    #endregion
}
