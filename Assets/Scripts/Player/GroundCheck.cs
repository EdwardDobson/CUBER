using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField]
    bool m_grounded;
    [SerializeField]
    float m_groundDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }
    void Check()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, m_groundDistance);
        if(hit.collider != null)
        {
            if (hit.collider.tag.Contains("Platform"))
            {
                m_grounded = true;
            }
    
            Debug.DrawRay(transform.position, -Vector2.up);

        }
        else m_grounded = false;

    }
    public bool isGrounded()
    {
        return m_grounded;
    }
    public void setIsGround(bool _state)
    {
        m_grounded = _state;
    }
}
