using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMainMenu : MonoBehaviour
{
    private static DontDestroyMainMenu Instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
