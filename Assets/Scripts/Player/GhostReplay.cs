using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostReplay : MonoBehaviour
{
    [SerializeField]
    List<Vector2> m_playerPositions = new List<Vector2>();
    Coroutine m_moveIE;
    [SerializeField]
    float m_speed = 0;
    [SerializeField]
    float m_addPosTimerMax;
    float m_addPosTimerCurrent;
    List<Input> m_playerInputs = new List<Input>();
    [SerializeField]
    List<float> m_horizontalValues = new List<float>();
    Rigidbody2D m_rb2d;
    // Start is called before the first frame update
    void Start()
    {
        m_addPosTimerCurrent = m_addPosTimerMax;
     //   m_rb2d = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.G))
        {
          //  Move();
        StartCoroutine(MoveObject());
        }
        if (Input.GetKeyDown(KeyCode.R))
            m_playerPositions.Clear();
    }
    void Move()
    {
        if(m_horizontalValues.Count > 0)
        {
            for (int i = 0; i < m_horizontalValues.Count; ++i)
            {
                float h = m_horizontalValues[i];
                m_rb2d.AddForce(new Vector2(h, 0) * m_speed * 0.2f);
                Debug.Log("Ghost move");
            }
        }
    
    }
    IEnumerator MoveObject()
    {
        for (int i = 0; i < m_playerPositions.Count; ++i)
        {

            m_moveIE = StartCoroutine(Moving(i));
            yield return m_moveIE;
        }
    }
    IEnumerator Moving(int _index)
    {
        while((Vector2)transform.position != m_playerPositions[_index])
        {
            transform.position = Vector2.MoveTowards(transform.position, m_playerPositions[_index], m_speed * Time.deltaTime);
            yield return null;
        }
    }
    public void AddPositions(Vector2 _pos)
    {
        if(!m_playerPositions.Contains(_pos))
        {
            m_addPosTimerCurrent -= Time.deltaTime;
                if(m_addPosTimerCurrent < 0)
            {
                m_playerPositions.Add(_pos);
                m_addPosTimerCurrent = m_addPosTimerMax;
            }


        }
    }
    public void AddHorizontalValues(float _value)
    {
        if (!m_horizontalValues.Contains(_value))
        {
            m_addPosTimerCurrent -= Time.deltaTime;
            if (m_addPosTimerCurrent < 0)
            {
                m_horizontalValues.Add(_value);
                m_addPosTimerCurrent = m_addPosTimerMax;
            }
        }
    }
}
