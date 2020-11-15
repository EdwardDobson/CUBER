using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum FireType
{
    Hitscan,
    Projectile
}
public class EnemyShooting : MonoBehaviour
{
    public FireType ShootingType;
    [SerializeField]
    Collider2D m_player;
    [SerializeField]
    float m_detectionRange;
    bool m_foundPlayer;
    [SerializeField]
    float m_maxFireRate;
    float m_currentFireRate;
    [SerializeField]
    GameObject m_bulletPrefab;
    [SerializeField]
    int m_shotDamage;
    LineRenderer m_laserEffect;
    [SerializeField]
    Vector3[] m_positions = new Vector3[2];
    ParticleSystem m_shootingParticles;
    // Start is called before the first frame update
    void Start()
    {
        m_currentFireRate = m_maxFireRate;
        m_laserEffect = GetComponent<LineRenderer>();
        m_player = GameObject.Find("Player").GetComponent<Collider2D>();
        m_shootingParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    void Update()
    {
        DetectPlayer();
        Aim();
    }
    void DetectPlayer()
    {
        Collider2D[] cols =  Physics2D.OverlapCircleAll(transform.position, m_detectionRange);
        foreach(Collider2D c in cols)
        {
            if (c.gameObject.tag.Contains("Player"))
                m_foundPlayer = true;
        }
        if (!cols.Contains(m_player))
        {
            m_positions[0] = new Vector3(0, 0, 0);
            m_positions[1] = new Vector3(0, 0, 0);
            m_laserEffect.SetPositions(m_positions);
            m_foundPlayer = false;
        }
    }
    void Aim()
    {
        if(m_foundPlayer)
            Attack();
    }
    void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -(transform.position - m_player.transform.position), m_detectionRange);
        Debug.DrawRay(transform.position, -(transform.position - m_player.transform.position));
        float missFactor = (transform.position - m_player.transform.position).sqrMagnitude;
        m_positions[0] = transform.position;
        m_positions[1] = m_player.transform.position;
        float RandomMiss = Random.Range(0, missFactor);

        if (hit.collider !=null)
        {
            if(hit.transform.gameObject.tag.Contains("Player") && !hit.transform.gameObject.tag.Contains("Room"))
            {
                m_laserEffect.SetPositions(m_positions);
                m_currentFireRate -= Time.deltaTime;
                if(m_currentFireRate <= 0)
                {
                    m_shootingParticles.Play();
                    switch (ShootingType)
                    {
                        case FireType.Hitscan:
                            if(RandomMiss <= 6)
                            {
                                Debug.Log("Attack Player Hit Scan");
                                hit.transform.GetComponent<PlayerStats>().TakeDamage(m_shotDamage);
                            }
                            else
                            {
                                Debug.Log("Attack Player Hit Scan Missed");
                            }
                            break;
                        case FireType.Projectile:
                            SpawnBullet();
                            Debug.Log("Attack Player Projectile");
                            break;
                        default:
                            break;
                    }
                    m_currentFireRate = m_maxFireRate;
                }
            }
            else
            {
                m_currentFireRate = m_maxFireRate;
                m_positions[0] = new Vector3(0, 0, 0);
                m_positions[1] = new Vector3(0, 0, 0);
                m_laserEffect.SetPositions(m_positions);
            }
        }
    }
    void SpawnBullet()
    {
        GameObject clone = Instantiate(m_bulletPrefab,transform.position,Quaternion.identity);
        clone.GetComponent<Bullet>().SetDirection(-(transform.position - m_player.transform.position).normalized);
        clone.GetComponent<Bullet>().SetDamage(m_shotDamage);
    }
}
