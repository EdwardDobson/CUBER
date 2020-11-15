using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float m_duration;
    [SerializeField]
    float m_speed;
    [SerializeField]
    Rigidbody2D m_rb2d;
    Vector2 m_bulletDir;
    int m_damage;
    // Start is called before the first frame update
    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        Move();
    }
    private void Update()
    {
  
        DetectObject();
    }
    void Move()
    {
        transform.right = m_bulletDir;
        m_rb2d.velocity = transform.right * m_speed;

    }
    public void SetDirection(Vector2 _dir)
    {
        m_bulletDir = _dir;
    }
    void DetectObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, m_bulletDir, 0.2f);
        Debug.DrawRay(transform.position, m_bulletDir);
        if(hit.collider != null)
        {
            if(hit.collider.gameObject.tag.Contains("Player"))
            {
                hit.transform.GetComponent<PlayerStats>().TakeDamage(m_damage);
                Destroy(gameObject);
            }
   
        }
        Collider2D[] col = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size,360f);
        Collider2D t = col.Where(c => !c.tag.Contains("Player") && !c.tag.Contains("Enemy") && !c.tag.Contains("Bullet") && !c.tag.Contains("Room")).FirstOrDefault();
        if(t != null)
        {
            Destroy(gameObject);
        }
    }
    public void SetDamage(int _value)
    {
        m_damage = _value;
    }

}
