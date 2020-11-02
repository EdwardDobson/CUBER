using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    public List<Vector2> Points = new List<Vector2>();
    [SerializeField]
    float m_speed;
    [SerializeField]
    int m_repeatRateSpeed;
    [SerializeField]
    int m_damage;
    bool m_nearPlayer;
    PlayerStats m_player;
    [SerializeField]
    float m_maxAttackSpeed;
    float m_currentAttackSpeed;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.Find("Player").GetComponent<PlayerStats>();
      GameObject g =  GameObject.Find("WalkingEnemyPoints");
        Points.Add(transform.position);
        foreach (Transform t in g.transform)
        {
            if(t.gameObject.name.Contains(gameObject.name))
            Points.Add(t.position);
        }
        m_currentAttackSpeed = m_maxAttackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(Points[0], Points[1], Mathf.PingPong(Time.time * m_speed, m_repeatRateSpeed));
        if(m_nearPlayer)
        {
            m_currentAttackSpeed -= Time.deltaTime;
            if(m_currentAttackSpeed <= 0)
            {
                m_player.TakeDamage(m_damage);
                m_currentAttackSpeed = m_maxAttackSpeed;
            }
  
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag.Contains("Player"))
        {
            m_nearPlayer = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            m_nearPlayer = true;
            collision.GetComponent<PlayerStats>().TakeDamage(m_damage);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            m_nearPlayer = false;
            m_currentAttackSpeed = m_maxAttackSpeed;
        }
    }
}
