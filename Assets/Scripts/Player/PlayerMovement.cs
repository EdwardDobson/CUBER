using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
    // Start is called before the first frame update
    GhostReplay m_replay;
    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_groundCheck = GetComponent<GroundCheck>();
        m_wallMask = LayerMask.GetMask("Wall");
        m_replay = GameObject.Find("Ghost").GetComponent<GhostReplay>();
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontal = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && m_groundCheck.isGrounded() && !m_isJumping)
        {
            m_isJumping = true;
        }
 
        WallJumping();
        m_replay.AddPositions(transform.position);
        Sliding();
        if (m_groundCheck.isGrounded() && !m_isJumping && !m_isSliding)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
            //   m_replay.AddHorizontalValues(m_horizontal);
     }
    private void FixedUpdate()
    {
        if (m_groundCheck.isGrounded() == true)
        {
            MoveOnGround();
        }
        else
        {
            MoveInAir();
            m_isSliding = false;
        }
        if (m_isJumping)
            Jump();
        WallGrab();
    }
    void MoveOnGround()
    {

        m_rb2d.AddForce(new Vector2(m_horizontal, 0) * m_speed * m_groundSpeedMultiplier);
        m_rb2d.velocity = Vector2.ClampMagnitude(m_rb2d.velocity, m_maxVelo);
        if (m_horizontal < 0)
        {
            m_frontCheck.position = new Vector2(transform.position.x - 0.1f, transform.position.y);
        }
        else
        {
            m_frontCheck.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
        }
    }
    void MoveInAir()
    {

        m_rb2d.AddForce(new Vector2(m_horizontal, 0) * m_speed * m_airSpeedMultiplier);
 
        if (m_horizontal < 0)
        {
            m_frontCheck.position = new Vector2(transform.position.x - 0.1f, transform.position.y);

        }
        else
        {
            m_frontCheck.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
        }
    }
    void Jump()
    {

        Debug.Log("Jumping");
        m_groundCheck.setIsGround(false);

        m_rb2d.AddForce(new Vector2(0, m_jumpForce));
        m_rb2d.velocity = Vector2.ClampMagnitude(m_rb2d.velocity, m_maxVelo);
        SquishEffectJump();
        m_isJumping = false;
        m_isSliding = false;
    }

    void SquishEffectJump()
    {
         transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, m_jumpStretchSize.y, transform.localScale.y), transform.localScale.z);
    }
    void Sliding()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !m_isSliding && m_groundCheck.isGrounded())
        {
            m_speed = m_speed* m_speedBoost;
            SlideSquishDown();
            m_isSliding = true;
            m_isJumping = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && m_isSliding && m_groundCheck.isGrounded())
        {
            m_speed = m_speed / m_speedBoost;
            SlideSquishUp();
            m_isSliding = false;
        }
    }
    void SlideSquishDown()
    {
         transform.localScale = new Vector3(1, 0.8f, 1);
        Debug.Log("Sliding");
    }
    void SlideSquishUp()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
    void WallGrab()
    {
        m_grabbedWall = Physics2D.OverlapCircle(m_frontCheck.position, m_checkRadius, m_wallMask);

        if (m_grabbedWall && !m_groundCheck.isGrounded() && m_horizontal != 0)
        {
            m_wallSliding = true;

        }
        else
        {
            m_wallSliding = false;
        }
        if (m_wallSliding)
        {
            m_rb2d.velocity = new Vector2(m_rb2d.velocity.x, Mathf.Clamp(m_rb2d.velocity.y, -m_wallSlidingSpeed, float.MaxValue));
        }
        if (m_wallJumping)
        {
            if(Input.GetKey(KeyCode.A))
            {
                m_rb2d.velocity = new Vector2(m_xWallForce * Vector2.left.x, m_yWallForce);
            }
            if (Input.GetKey(KeyCode.D))
            {
                m_rb2d.velocity = new Vector2(m_xWallForce * Vector2.right.x, m_yWallForce);
            }
        }
        Debug.Log("Wall Grabbing");
    }
    void WallJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_wallSliding)
        {
            m_wallJumping = true;
            Invoke("SetWallJumpingFalse", m_wallJumpTime);
        }
    }
    void SetWallJumpingFalse()
    {
        m_wallJumping = false;
    }
}
