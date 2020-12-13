using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    bool m_playerEntered;
    GameObject m_player;
    private void Start()
    {
        m_player = GameObject.Find("Player");
    }
    private void Update()
    {
        if(m_playerEntered)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TP(m_player);
                transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Play();
            }
        }

    }
    void TP(GameObject _player)
    {
        _player.transform.position = transform.GetChild(1).position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Contains("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            m_playerEntered = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
      
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            m_playerEntered = false;
        }
    }
}
