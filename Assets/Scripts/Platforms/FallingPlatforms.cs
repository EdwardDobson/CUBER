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
    float CoolDown;
    // Start is called before the first frame update
    void Start()
    {
        m_startPoint = transform.position;
        CoolDown = CoolDownMax;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case PlatformState.Idle:

                break;
            case PlatformState.Falling:
                transform.position = Vector2.MoveTowards(transform.position, EndPoint, FallSpeed * Time.deltaTime);
                if((Vector2)transform.position == EndPoint)
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
                if((Vector2)transform.position == m_startPoint)
                {
                 State =    PlatformState.Idle;
                }
                break;
        }

    }
}
