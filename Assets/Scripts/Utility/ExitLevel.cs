using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ExitLevel : MonoBehaviour
{
    public int NextLevelIndex;
    bool m_stopTakingInput;
    bool m_playerIn;
    bool m_playerInLevelExit;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                m_playerIn = true;
                if (transform.childCount > 1)
                {
                    transform.GetChild(2).gameObject.SetActive(true);
                }

            }
            if (SceneManager.GetActiveScene().buildIndex > 1)
            {
                m_playerInLevelExit = true;
                transform.GetChild(0).GetChild(1).GetComponent<TextMeshPro>().text = "Press E";
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            if (m_playerIn)
                m_playerIn = false;
            if (transform.childCount > 1)
            {
                transform.GetChild(2).gameObject.SetActive(false);
            }
            if (transform.childCount == 1)
            {
                transform.GetChild(0).GetChild(1).GetComponent<TextMeshPro>().text = "";
            }
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
        if (m_playerInLevelExit)
        {
            if (Input.GetKeyDown(KeyCode.E) )
            {
                GameObject.Find("GameManager").GetComponent<ScoreManager>().SetDataToSave();
            }
        }
    }
    IEnumerator PlayLevelEndEffect()
    {
        if (transform.childCount > 1)
        {
            transform.GetChild(2).gameObject.SetActive(false);
        }
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
    
        while (!asyncLoad.isDone)
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
