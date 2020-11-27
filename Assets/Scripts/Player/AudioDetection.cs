using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDetection : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip[] Clips;
    PlayerStats m_playerStats;
    private void Start()
    {
        m_playerStats = GetComponent<PlayerStats>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Contains("Score"))
        {
            Source.PlayOneShot(Clips[0]);
            GameObject newGameObject = Instantiate(collision.transform.GetChild(0).gameObject, null);
            newGameObject.transform.position = collision.transform.position;
            newGameObject.GetComponent<ParticleSystem>().Play();
            Destroy(newGameObject, newGameObject.GetComponent<ParticleSystem>().main.duration * 2);
        }
        if (collision.tag.Contains("Health"))
        {
            if (collision.transform.childCount > 0 && m_playerStats.GetCurrentHealth() < m_playerStats.GetMaxHealth())
            {
                Source.PlayOneShot(Clips[1]);
                GameObject newGameObject = Instantiate(collision.transform.GetChild(0).gameObject, null);
                newGameObject.transform.position = collision.transform.position;
                newGameObject.GetComponent<ParticleSystem>().Play();
                Destroy(newGameObject, newGameObject.GetComponent<ParticleSystem>().main.duration * 2);
            }
        
        }
        if (collision.tag.Contains("Shield"))
        {
            if (collision.transform.childCount > 0)
            {
                Source.PlayOneShot(Clips[2]);
                GameObject newGameObject = Instantiate(collision.transform.GetChild(0).gameObject, null);
                newGameObject.transform.position = collision.transform.position;
                newGameObject.GetComponent<ParticleSystem>().Play();
                Destroy(newGameObject, newGameObject.GetComponent<ParticleSystem>().main.duration * 2);
            }
        }
        if(collision.tag.Contains("Key"))
        {
            if (collision.transform.childCount > 0)
            {
                Source.PlayOneShot(Clips[3]);
                GameObject newGameObject = Instantiate(collision.transform.GetChild(0).gameObject, null);
                newGameObject.transform.position = collision.transform.position;
                newGameObject.GetComponent<ParticleSystem>().Play();
                Destroy(newGameObject, newGameObject.GetComponent<ParticleSystem>().main.duration * 2);
            }
        }
        if (collision.tag.Contains("DoorToOpen"))
        {

        }
    }
   public void PlayDoorSound(Collider2D _collision, DoorColour _colour, int _keyValue)
    {
        if (_collision.transform.childCount > 0)
        {
            if (_collision.GetComponent<DoorOpen>().doorColour == _colour && _keyValue > 0)
            {
                Source.PlayOneShot(Clips[3]);
                GameObject newGameObject = Instantiate(_collision.transform.GetChild(0).gameObject, null);
                newGameObject.transform.position = _collision.transform.position;
                newGameObject.GetComponent<ParticleSystem>().Play();
                Destroy(newGameObject, newGameObject.GetComponent<ParticleSystem>().main.duration * 2);
                Destroy(_collision.gameObject);

            }

        }
    }
}
