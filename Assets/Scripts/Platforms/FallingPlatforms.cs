using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlatformState
{
    Idle,
    Falling,
    Recovering

}


public class FallingPlatforms : MonoBehaviour
{
    public Vector2 EndPoint;
    Vector2 m_startPoint;
    public float RecoverySpeed;
    public float FallSpeed;
    public PlatformState State;
    public float CoolDownMax;
    public float TimeBeforeFallMax;
     float TimeBeforeFall;
    float CoolDown;
    // Start is called before the first frame update
    void Start()
    {
        m_startPoint = transform.position;
        CoolDown = CoolDownMax;
        TimeBeforeFall = TimeBeforeFallMax;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case PlatformState.Idle:
                GetComponent<BoxCollider2D>().enabled = true;
                break;
            case PlatformState.Falling:
                transform.position = Vector2.MoveTowards(transform.position, EndPoint, FallSpeed * Time.deltaTime);
                GetComponent<BoxCollider2D>().enabled = false;
                if ((Vector2)transform.position == EndPoint)
                {
                    CoolDown -= Time.deltaTime;
                    if (CoolDown <= 0)
                    {
                        CoolDown = CoolDownMax;
                        State = PlatformState.Recovering;
                    }
                }
      
                break;
            case PlatformState.Recovering:
                transform.position = Vector2.MoveTowards(transform.position, m_startPoint, RecoverySpeed * Time.deltaTime);
                GetComponent<BoxCollider2D>().enabled = false;
                if((Vector2)transform.position == m_startPoint)
                {
                     State =    PlatformState.Idle;
                }
                break;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Player"))
        {
            TimeBeforeFall -= Time.deltaTime;
            if(TimeBeforeFall <= 0)
            {
                State = PlatformState.Falling;
                TimeBeforeFall = TimeBeforeFallMax;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            TimeBeforeFall = TimeBeforeFallMax;
        }
    }
}
