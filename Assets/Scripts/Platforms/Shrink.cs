using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    Vector2 m_pos;
    float m_range;
   public float minSize;
  public  float maxSize;
    // Start is called before the first frame update
    void Start()
    {
        m_pos = transform.position;
        m_range = minSize - maxSize;

    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale =new Vector3( minSize + Mathf.PingPong(Time.time * 2, m_range),transform.localScale.y,transform.localScale.z);
    }
}
