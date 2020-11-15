using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Transform m_player;
    float m_speed;
    int m_damage;
    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, m_player.position, m_speed * Time.deltaTime);
        FireRay();
        Vector2 dir = transform.position - m_player.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    public Transform SetPlayer(Transform _player)
    {
     return   m_player = _player;
    }
    public float SetSpeed(float _speed)
    {
        return m_speed = _speed;
    }
    public int SetDamage(int _damage)
    {
        return m_damage = _damage;
    }

    void FireRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -(transform.position - m_player.position), 0.3f);
        Debug.DrawRay(transform.position, -(transform.position - m_player.position));
        if(hit.collider != null)
        {
            if(hit.transform.gameObject.tag.Contains("Player"))
            {
                hit.transform.GetComponent<PlayerStats>().TakeDamage(m_damage);
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
