using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public BasePickup PickupObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Contains("Player"))
        {
            if(!PickupObject.ShouldReduceValue)
            {
                collision.gameObject.GetComponent<PlayerStats>().IncreaseStats(PickupObject.PickupType, gameObject);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerStats>().ReduceStats(PickupObject.PickupType, gameObject);
            }
        }
    }
}
