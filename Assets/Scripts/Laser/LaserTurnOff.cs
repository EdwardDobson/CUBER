using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurnOff : MonoBehaviour
{
    float m_laserDurationCurrent;
    public float LaserDurationMax;
    float m_CoolDownCurrent;
    public float CoolDownMax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_laserDurationCurrent -= Time.deltaTime;
        if (m_laserDurationCurrent <= 0)
        {
            m_laserDurationCurrent = 0;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            m_CoolDownCurrent -= Time.deltaTime;
            if (m_CoolDownCurrent <= 0)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<BoxCollider2D>().enabled = true;
                m_CoolDownCurrent = CoolDownMax;
                m_laserDurationCurrent = LaserDurationMax;
            }
        }
    }
}
