using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public TextMeshPro GotCheckPointText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().SetSpawnPoint(transform.position);
            GetComponent<BoxCollider2D>().enabled = false;
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
