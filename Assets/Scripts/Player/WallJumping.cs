using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WallJumpingStates
{
    Idle,
    Grabbing,
    Sliding,
    Jumping
}
public class WallJumping : MonoBehaviour
{
    bool m_sliding;
    bool m_grabbedWall;
    bool m_wallJumping;
    Rigidbody2D m_rb2d;
    Vector2 m_oldVelo;
    WallJumpingStates m_state;
    GroundCheck m_groundCheck;
    Transform m_frontCheck;
    [SerializeField]
    float m_checkRadius;
    [SerializeField]
    float m_wallSlidingSpeed;
    [SerializeField]
    float m_xWallForce;
    [SerializeField]
    float m_yWallForce;
    float m_horizontal;
    [SerializeField]
    float m_wallJumpTime;
    LayerMask m_wallMask;
    private void Start()
    {
        m_groundCheck = GetComponent<GroundCheck>();
        m_frontCheck = transform.GetChild(0);
        m_rb2d = GetComponent<Rigidbody2D>();
        m_horizontal = GetComponent<PlayerMovement>().GetHorizontal();
        
    }
    private void FixedUpdate()
    {
        if (!m_groundCheck.isGrounded())
            m_state = WallJumpingStates.Grabbing;
        switch (m_state)
        {
            case WallJumpingStates.Idle:
                break;
            case WallJumpingStates.Grabbing:
                Grabbing();
                break;
            case WallJumpingStates.Jumping:
                Jumping();
                break;
            case WallJumpingStates.Sliding:
                Sliding();
                break;
            default:
                break;
        }
    }
    void Grabbing()
    {
        m_grabbedWall = Physics2D.OverlapCircle(m_frontCheck.position, m_checkRadius, m_wallMask);
        if (m_grabbedWall && !m_groundCheck.isGrounded() && m_horizontal != 0)
        {
            m_state = WallJumpingStates.Sliding;
            Debug.Log("Grabbing");
        }
    }
    void JumpingInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_state = WallJumpingStates.Jumping;
        }
    }
    void Jumping()
    {
        Invoke("SetWallJumpingFalse", m_wallJumpTime);
        if (m_oldVelo.x == 0)
            m_rb2d.velocity = new Vector2(m_xWallForce * m_horizontal, m_yWallForce);
        else
        {
            if (m_oldVelo.x < 0)
                m_rb2d.velocity = new Vector2(-m_oldVelo.x * m_horizontal, m_yWallForce);
            else
                m_rb2d.velocity = new Vector2(m_oldVelo.x * m_horizontal, m_yWallForce);
        }
//        MakeDust();
    }
    void Sliding()
    {
        m_rb2d.velocity = new Vector2(m_rb2d.velocity.x, Mathf.Clamp(m_rb2d.velocity.y, -m_wallSlidingSpeed, float.MaxValue));
        JumpingInput();
    }
    void SetWallJumpingFalse()
    {
        m_wallJumping = false;
    }
}
