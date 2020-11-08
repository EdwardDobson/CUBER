using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RotateDirection
{
    Right,
    Left,
}
public class Rotate : MonoBehaviour
{
    public float Speed;
    public RotateDirection dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(dir)
        {
            case RotateDirection.Left:
                transform.Rotate(Vector3.forward, 45 * Time.deltaTime * Speed);
                break;
            case RotateDirection.Right:
                transform.Rotate(Vector3.back, 45 * Time.deltaTime * Speed);
                break;
        }
  
    }
}
