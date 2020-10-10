using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D m_rb2d;
    [SerializeField]
    float m_speed;
    [SerializeField]
    float m_jumpForce;
    [SerializeField]
    float m_groundSpeedMultiplier;
    [SerializeField]
    float m_airSpeedMultiplier;
    GroundCheck m_groundCheck;

    // Start is called before the first frame update
    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_groundCheck = GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_groundCheck.isGrounded() == true)
        {
            MoveOnGround();
        }
        else MoveInAir();

        Jump();
    }
    void MoveOnGround()
    {
        float H = Input.GetAxis("Horizontal");
        m_rb2d.velocity = new Vector2(H, 0) * m_speed * m_groundSpeedMultiplier;
    }
    void MoveInAir()
    {
        float H = Input.GetAxis("Horizontal");
        m_rb2d.AddForce(new Vector2(H, 0) * m_speed * m_airSpeedMultiplier);
    }
    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && m_groundCheck.isGrounded() == true)
        {
            Debug.Log("Jumping");
            m_groundCheck.setIsGround(false);
            m_rb2d.AddForce(new Vector2(0, m_jumpForce));
        }
    }
}
