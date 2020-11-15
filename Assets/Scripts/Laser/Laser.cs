using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LaserType
{
    Spinning,
    Straight
}
public class Laser : MonoBehaviour
{
    public LaserType LaserType;
    public RotateDirection RotationDirection;
    public float MoveSpeed;
    public float RotateSpeed;

    [SerializeField]
    bool m_moveBack;
    [SerializeField]
    Vector2 m_startPos;
    [SerializeField]
    Vector2 m_endPos;
    // Start is called before the first frame update
    void Start()
    {
        m_startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (LaserType == LaserType.Straight)
        {
            if ((Vector2)transform.position != m_endPos && !m_moveBack)
                transform.position = Vector2.MoveTowards(transform.position, m_endPos, MoveSpeed * Time.deltaTime);
            if((Vector2)transform.position == m_endPos)
                m_moveBack = true;
            if(m_moveBack)
                transform.position = Vector2.MoveTowards(transform.position, m_startPos, MoveSpeed * Time.deltaTime);
            if ((Vector2)transform.position == m_startPos)
                m_moveBack = false;
        }
        else
        {
            switch (RotationDirection)
            {
                case RotateDirection.Left:
                    transform.Rotate(Vector3.forward, 45 * Time.deltaTime * RotateSpeed);
                    break;
                case RotateDirection.Right:
                    transform.Rotate(Vector3.back, 45 * Time.deltaTime * RotateSpeed);
                    break;
            }
        }
    }
}
