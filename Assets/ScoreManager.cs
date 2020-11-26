
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[Serializable]
public struct SaveData
{
    public int Score;
    public int StarCount;
    public int LevelIndex;
    public string Nickname;
}
public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    SaveData _data = new SaveData();
    public TextMeshProUGUI HighestScores;
    public TextMeshProUGUI Positions;
    public TextMeshProUGUI Stars;
    public TextMeshProUGUI Nicknames;
    public TMP_Dropdown LevelSelector;
    SaveObj m_saveObject;
    PlayerStats m_player;
    public GameObject WinScreen;
    public bool ShouldSave;
    public void SetDataToSaveUI()
    {
        WinScreen.SetActive(true);
        _data = new SaveData();
        m_player = GameObject.Find("Player").GetComponent<PlayerStats>();
        WinScreen.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = m_player.GetScore().ToString();
        _data.Score = m_player.GetScore();
        _data.Nickname = WinScreen.transform.GetChild(9).GetComponent<TMP_InputField>().text;
        if (m_player.CheckIfDied())
        {
            WinScreen.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
            _data.StarCount += 1;
        }
        else
            WinScreen.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
        if (m_player.GetBigStarCollected())
        {
            _data.StarCount += 1;
            WinScreen.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else
            WinScreen.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
        if (m_player.GetPointThresholdMet())
        {
            WinScreen.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.green;
            _data.StarCount += 1;
        }
        else
            WinScreen.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.red;
        WinScreen.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = _data.StarCount.ToString();
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                _data.LevelIndex = 1;
                break;
            case 3:
                _data.LevelIndex = 2;
                break;
            case 4:
                _data.LevelIndex = 3;
                break;
        }
    }
    public void SetDataToSave()
    {
       
        if(ShouldSave)
        SaveToFile();
    }
    public void SetShouldSave(bool _state)
    {
       ShouldSave = _state;
    }
  public void SaveToFile()
    {
        if(File.Exists("Assets/Resources/Save/SaveData.asset"))
        {
            if (m_saveObject == null)
                m_saveObject = (SaveObj)Resources.Load<ScriptableObject>("Save/SaveData");
            m_saveObject.datas.Add(_data);
        }
    }
    public  void LoadIntoLeaderBoard()
    {
        Positions.text = "";
        HighestScores.text = "";
        Stars.text = "";
        Nicknames.text = "";
        if (m_saveObject == null)
            m_saveObject = (SaveObj)Resources.Load<ScriptableObject>("Save/SaveData");
        List<SaveData> tempDatas = m_saveObject.datas.Where(s => s.LevelIndex == LevelSelector.value + 1).ToList();
        tempDatas = tempDatas.OrderByDescending(s => s.Score).ToList();
        if (tempDatas.Count > 10)
        {
            tempDatas.RemoveRange(10, m_saveObject.datas.Count - 10);
        }
        if (tempDatas.Count > 0)
        {
            for (int i = 0; i < tempDatas.Count; ++i)
            {
                Positions.text += (i + 1).ToString();
                Positions.text += "\n";
                HighestScores.text += tempDatas[i].Score.ToString();
                HighestScores.text += "\n";
                Nicknames.text += tempDatas[i].Nickname;
                Nicknames.text += "\n";
                for (int a = 0; a < tempDatas[i].StarCount; ++a)
                {
                    Stars.text += "<sprite index= 0>";
                }
                Stars.text += "\n";
            }
        }
     }
}
