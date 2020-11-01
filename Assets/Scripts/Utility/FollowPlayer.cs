using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Player;
    [SerializeField]
    Vector3 m_offset;
    [SerializeField]
    List<Vector3> m_cameraPositions = new List<Vector3>();
    Door[] m_doors;
    private void Start()
    {
        foreach(Transform t in transform)
        {
            m_cameraPositions.Add(t.position);
        }
        transform.position = m_cameraPositions[0];
        m_doors = FindObjectsOfType<Door>();
    }
    void FixedUpdate()
    {
        for (int i = 0; i < m_doors.Length; ++i)
        {
            if(m_doors[i].Active)
            transform.position = m_cameraPositions[m_doors[i].RoomNumber];
        }
        //  if(Player != null)
        //    transform.position = new Vector3(Player.position.x , Player.position.y , m_offset.z);
    }

}
