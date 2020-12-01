
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SaveHolder
{
    public List<SaveData> datas = new List<SaveData>();
}
[Serializable]
public struct SaveData
{
    public List<int> Scores;
    public List<int> StarCount;
    public List<int> LevelIndex;
    public List<string> Nickname;
}

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI HighestScores;
    public TextMeshProUGUI Positions;
    public TextMeshProUGUI Stars;
    public TextMeshProUGUI Nicknames;
    public TMP_Dropdown LevelSelector;
    PlayerStats m_player;
    public GameObject WinScreen;
    public bool ShouldSave;
    int m_levelIndex;
    int m_starAmount;
    int m_levelSelectIndex = 1;
    string m_json;
    SaveData newData;
    SaveData dataToAdd;
    [SerializeField]
    SaveHolder m_holder;
    private void Start()
    {
        newData = new SaveData
        {
            Scores = new List<int>(),
            LevelIndex = new List<int>(),
            Nickname = new List<string>(),
            StarCount = new List<int>(),
        };
        dataToAdd = new SaveData
        {
        };
        m_holder = new SaveHolder();
    }
    public void SaveDataVisual()
    {
        if (WinScreen != null)
        {
            m_player = GameObject.Find("Player").GetComponent<PlayerStats>();
            WinScreen.SetActive(true);
            WinScreen.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = m_player.GetScore().ToString();
            if (m_player != null)
            {
                if (m_player.CheckIfDied())
                {
                    WinScreen.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
                }
                else
                {
                    WinScreen.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
                }
                if (m_player.GetBigStarCollected())
                {
                    WinScreen.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.green;
                }
                else
                {
                    WinScreen.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
                }
                if (m_player.GetPointThresholdMet())
                {
                    WinScreen.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.green;
                }
                else
                {
                    WinScreen.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.red;
                }

            }
        }
    }
    public void SaveData()
    {
        m_starAmount = 0;
        if (File.Exists(Application.persistentDataPath + "/save.txt"))
        {
            string s = File.ReadAllText(Application.persistentDataPath + "/save.txt");
            if (s != "")
            {
                SaveData loadedData = JsonUtility.FromJson<SaveData>(s);
                newData.Scores = loadedData.Scores;
                newData.LevelIndex = loadedData.LevelIndex;
                newData.StarCount = loadedData.StarCount;
                newData.Nickname = loadedData.Nickname;
       
            }
        }
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                m_levelIndex = 1;
                break;
            case 3:
                m_levelIndex = 2;
                break;
            case 4:

                m_levelIndex = 3;
                break;
        }
        if (m_player.CheckIfDied())
            m_starAmount += 1;
        if (m_player.GetBigStarCollected())
            m_starAmount += 1;
        if (m_player.GetPointThresholdMet())
            m_starAmount += 1;
        newData.Scores.Add(m_player.GetScore());
        newData.LevelIndex.Add(m_levelIndex);
        newData.StarCount.Add(m_starAmount);
        if (WinScreen != null)
            newData.Nickname.Add(WinScreen.transform.GetChild(9).GetComponent<TMP_InputField>().text);
        m_json = JsonUtility.ToJson(newData, true);
            File.WriteAllText(Application.persistentDataPath + "/save.txt", m_json);
    }
    public bool LoadData()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            if(File.Exists(Application.persistentDataPath + "/save.txt"))
            {
                string savestring = File.ReadAllText(Application.persistentDataPath + "/save.txt");
                if (savestring != "")
                {
                    SaveData loadedData = JsonUtility.FromJson<SaveData>(savestring);
                    dataToAdd.Scores = loadedData.Scores;
                    dataToAdd.StarCount = loadedData.StarCount;
                    dataToAdd.LevelIndex = loadedData.LevelIndex;
                    dataToAdd.Nickname = loadedData.Nickname;
                    AddToHolder();
                    return true;
                }
            }
        }
        return false;
    }
    void AddToHolder()
    {
        m_holder.datas.Clear();
        int amount = dataToAdd.Scores.Count;
        for (int i = 0; i < amount; i++)
        {
            SaveData n = new SaveData();
            n.Scores = new List<int>();
            n.StarCount = new List<int>();
            n.Nickname = new List<string>();
            n.LevelIndex = new List<int>();
            n.Scores.Add(dataToAdd.Scores[i]);
            n.Nickname.Add(dataToAdd.Nickname[i]);
            n.LevelIndex.Add(dataToAdd.LevelIndex[i]);
            n.StarCount.Add(dataToAdd.StarCount[i]);

            m_holder.datas.Add(n);
        }
    }
    public void SetShouldSave(bool _state)
    {
        ShouldSave = _state;
    }
    public bool GetShouldSave()
    {
        return ShouldSave;
    }
    public void LeaderBoardLoad()
    {
        if (LoadData())
        {
            Positions.text = "";
            HighestScores.text = "";
            Stars.text = "";
            Nicknames.text = "";
            int starCount;
            m_holder.datas = m_holder.datas.OrderByDescending(s => s.Scores.Max()).ToList();
            if(m_holder.datas.Count > 10)
            {
                m_holder.datas.RemoveRange(10, m_holder.datas.Count - 10);
            }
            for (int i = 0; i < m_holder.datas.Count; ++i)
            {
                for (int ind = 0; ind < m_holder.datas[i].LevelIndex.Count; ++ind)
                {
                    if (m_holder.datas[i].LevelIndex[ind] == m_levelSelectIndex)
                    {
                        Positions.text += (i + 1).ToString() + "\n";
                        for (int sc = 0; sc < m_holder.datas[i].Scores.Count; ++sc)
                        {
                            HighestScores.text += m_holder.datas[i].Scores[sc].ToString() + "\n";
                        }
                        for (int n = 0; n < m_holder.datas[i].Nickname.Count; ++n)
                        {
                            Nicknames.text += m_holder.datas[i].Nickname[n] + "\n";
                        }
                        for (int s = 0; s < m_holder.datas[i].StarCount.Count; ++s)
                        {
                            starCount = m_holder.datas[i].StarCount[s];
                            for (int a = 0; a < starCount; ++a)
                            {
                                Stars.text += "<sprite index= 0>";
                            }
                            Stars.text += "\n";
                        }
                    }
                }
            }
        }
    }
    public void SetLevelPickIndex(int _value)
    {
        m_levelSelectIndex = _value;
    }
}
