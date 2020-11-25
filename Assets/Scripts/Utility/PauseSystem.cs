using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    bool m_paused;
    public GameObject PauseObj;
    public bool CanPause;
    private static PauseSystem Instance;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0 && CanPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !m_paused)
            {
                Paused();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && m_paused)
            {
                Resume();
            }
        }
      
    }
    void Paused()
    {
        m_paused = true;
        Time.timeScale = 0;
        Debug.Log("Paused");
        PauseObj.SetActive(true);
    }
  public  void Resume()
    {
        Debug.Log("Resume");
        m_paused = false;
        Time.timeScale = 1;
        PauseObj.SetActive(false);
    }
    public bool GetPauseState()
    {
        return m_paused;
    }
}
