using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float DetectionRange;
    public float AttackRange;
    bool m_foundPlayer;
    bool m_launchMissile;
    bool m_canLaunch;
    Transform m_player;
    public GameObject MissilePrefab;
    public float MissileSpeed;
    public int MissileDamage;
    [SerializeField]
    float m_coolDownMax;
    [SerializeField]
    float m_coolDownCurrent;

    void Start()
    {
        m_player = GameObject.Find("Player").transform;
        m_canLaunch = true;
        m_coolDownCurrent = m_coolDownMax;
    }

    void Update()
    {
        if(m_canLaunch)
        FindPlayer();
        if(m_launchMissile)
        {
            LaunchMissile();
           
        }
        if(!m_canLaunch)
        {
             m_coolDownCurrent -= Time.deltaTime;
            if(m_coolDownCurrent <= 0)
            {
                m_canLaunch = true;
                m_coolDownCurrent = m_coolDownMax;
          
            }
        }

    }
    void FindPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, DetectionRange);
        foreach (Collider2D c in colliders)
        {
            if(c.gameObject.tag.Contains("Player"))
            {
                m_foundPlayer = true;
            }
        }
        if(m_foundPlayer)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -(transform.position - m_player.position), AttackRange);
            if(hit.collider != null)
            {
                if(hit.transform.gameObject.tag.Contains("Player"))
                {
                    m_launchMissile = true;

                }
            }
        }
    }
    void LaunchMissile()
    {
        GameObject clone = Instantiate(MissilePrefab,transform.position,Quaternion.identity);
        clone.transform.SetParent(transform);
        clone.GetComponent<Missile>().SetPlayer(m_player);
        clone.GetComponent<Missile>().SetSpeed(MissileSpeed);
        clone.GetComponent<Missile>().SetDamage(MissileDamage);
        m_canLaunch = false;
        m_launchMissile = false;
    }
}
