using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    public int NextLevelIndex;
    bool m_stopTakingInput;
    bool m_playerIn;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                m_playerIn = true;
            }
            else
            {
                LoadLevelInGame(NextLevelIndex);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            if (m_playerIn)
                m_playerIn = false;
  
        }
    }
    private void Update()
    {
        if(m_playerIn)
        {
            if (Input.GetKeyDown(KeyCode.E) && !m_stopTakingInput)
            {
                if (transform.childCount > 1)
                {
                    StartCoroutine(PlayLevelEndEffect());
                }
                else
                {
                    LoadLevelInGame(NextLevelIndex);
                }
                m_stopTakingInput = true;
            }
        }
    }
    IEnumerator PlayLevelEndEffect()
    {
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(transform.GetChild(1).GetComponent<ParticleSystem>().main.duration);
        LoadLevelInGame(NextLevelIndex);
    }
    public  void LoadLevelInGame(int _index)
    {
        StartCoroutine(LoadAsync(_index));
    }
    IEnumerator LoadAsync(int _index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_index);
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
        m_stopTakingInput = false;
    }
    public void RestartLevel()
    {
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
    }
 
}
