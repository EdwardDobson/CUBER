using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDetection : MonoBehaviour
{
    public AudioSource Source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Contains("Score"))
        {
            Source.PlayOneShot(Source.clip);
         GameObject newGameObject =  Instantiate(collision.transform.GetChild(0).gameObject, null);
            newGameObject.transform.position = collision.transform.position;
            newGameObject.GetComponent<ParticleSystem>().Play();
        }
    }
}
