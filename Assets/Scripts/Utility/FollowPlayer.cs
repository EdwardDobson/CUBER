using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FollowPlayer : MonoBehaviour
{
    public Transform Player;
    [SerializeField]
    Vector3 m_offset;
    [SerializeField]
    List<Vector3> m_cameraPositions = new List<Vector3>();
    Door[] m_doors;
    void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().buildIndex > 1)
        {
            for (int i = 0; i < m_doors.Length; ++i)
            {
                if (m_doors[i].Active)
                    transform.position = m_cameraPositions[m_doors[i].RoomNumber];
            }
        }
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex > 1)
        {
            Player = GameObject.Find("Player").transform;
            Transform cameraPoints = GameObject.Find("CameraPoints").transform;
            foreach (Transform t in cameraPoints)
            {
                m_cameraPositions.Add(t.position);
            }
            transform.position = m_cameraPositions[0];
            m_doors = FindObjectsOfType<Door>();
            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);
        }

    }
}
