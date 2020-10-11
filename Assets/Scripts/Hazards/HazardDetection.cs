using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDetection : MonoBehaviour
{
    public BaseHazard Hazard;
    bool m_startCoolDown;
    GameObject m_player;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Contains("Player"))
        {
            m_player = collision.gameObject;
        
            if (Hazard.isOverTimeAttack)
            {
                m_startCoolDown = true;
            }
            else
            {
                m_player.GetComponent<PlayerStats>().TakeDamage(Hazard.Damage);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            m_player = null;
            m_startCoolDown = false;
        }
    }
   private void Update()
    {
        if (m_startCoolDown)
        {
            Hazard.CoolDown -= Time.deltaTime;

            if (Hazard.CoolDown <= 0)
            {
                if(!Hazard.infiniteUses)
                    Hazard.Uses -= 1;
                m_player.GetComponent<PlayerStats>().TakeDamage(Hazard.Damage);
                Hazard.CoolDown = Hazard.MaxCoolDown;
            }    
            if (Hazard.Uses <= 0)
            {
                Hazard.CoolDown = Hazard.MaxCoolDown;
                Hazard.Uses = Hazard.MaxUses;
                Destroy(gameObject);
            }
        }
    }
}
