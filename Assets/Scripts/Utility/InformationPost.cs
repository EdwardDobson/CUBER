using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationPost : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
