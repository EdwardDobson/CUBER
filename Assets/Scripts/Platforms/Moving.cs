using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public List<Vector2> Points;
    public float Speed;
    int m_index;
    // Start is called before the first frame update
    void Awake()
    {
        m_index = 0;
    //    Points.Add(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_index < Points.Count)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, Points[m_index], step);
            if ((Vector2)transform.position == Points[m_index])
            {
                m_index += 1;
            }
        }
        if(m_index >= Points.Count)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, Points[0], step);
            if ((Vector2)transform.position == Points[0])
            {
                m_index = 0;
            }
        }
    
    }
}
