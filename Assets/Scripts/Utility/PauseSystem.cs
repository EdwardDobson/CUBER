using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    bool m_paused;
    public GameObject[] PauseObjs;
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
        if (SceneManager.GetActiveScene().buildIndex == 1)
            CanPause = true;
        if (SceneManager.GetActiveScene().buildIndex != 0 && CanPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !m_paused)
            {
                Paused();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && m_paused && PauseObjs[0].activeSelf && PauseObjs[1].activeSelf)
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
        PauseObjs[0].SetActive(true);
        PauseObjs[1].SetActive(true);
    }
  public  void Resume()
    {
        Debug.Log("Resume");
        m_paused = false;
        Time.timeScale = 1;
        PauseObjs[0].SetActive(false);
        PauseObjs[1].SetActive(false);
    }
    public bool GetPauseState()
    {
        return m_paused;
    }
}
