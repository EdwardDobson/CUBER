using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    public int NextLevelIndex;
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
        if (collision.tag.Contains("Player"))
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
    }
}
