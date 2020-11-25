using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public TextMeshPro GotCheckPointText;
    AudioSource m_checkPointSound;
    private void Start()
    {
        m_checkPointSound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().SetSpawnPoint(transform.position);
            GetComponent<BoxCollider2D>().enabled = false;
            m_checkPointSound.Play();
            StartCoroutine(ShowText());
        }
    }
    IEnumerator ShowText()
    {
        GotCheckPointText.text = "Checkpoint Reached!";
        yield return new WaitForSeconds(1);
        GotCheckPointText.text = "";
    }
}
