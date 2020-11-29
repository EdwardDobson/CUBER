using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ParticleType
{
    Jumping,
    Landing,
    Blood,
    Shield,
    GrappleHit,
    GrappleLaunch
}
public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] m_systems;
    // Start is called before the first frame update
    void Start()
    {
        m_systems = transform.GetComponentsInChildren<ParticleSystem>();
    }
    public void PlayEffect(ParticleType _type, bool _hasAudio = false)
    { 
        foreach(ParticleSystem ps in m_systems)
        {
            if(ps.gameObject.name == _type.ToString())
            {
                ps.Play();
                if (_hasAudio)
                    ps.GetComponent<AudioSource>().Play();
            }
        }
    }
}
