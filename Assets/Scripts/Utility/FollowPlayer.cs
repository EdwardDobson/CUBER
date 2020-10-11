using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Player;
    [SerializeField]
    Vector3 m_offset;
    void FixedUpdate()
    {
        if(Player != null)
        transform.position = new Vector3(Player.position.x , Player.position.y , m_offset.z);
    }
}
