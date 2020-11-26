using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldLimiter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMP_InputField>().characterLimit = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
