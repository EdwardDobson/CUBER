
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public struct SaveData
{
    public int Score;
    public int StarCount;
}
public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    SaveData _data = new SaveData();
    public TextMeshProUGUI HighestScores;
    public TextMeshProUGUI Positions;
    public TextMeshProUGUI Stars;
    SaveObj m_saveObject;
    PlayerStats m_player;
    public GameObject WinScreen;

    public void SetDataToSave()
    {
        WinScreen.SetActive(true);
        _data = new SaveData();
        m_player = GameObject.Find("Player").GetComponent<PlayerStats>();
        WinScreen.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = m_player.GetScore().ToString();
        _data.Score = m_player.GetScore();
        if(m_player.CheckIfDied())
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
        SaveToFile();
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
        if(m_saveObject == null)
            m_saveObject = (SaveObj)Resources.Load<ScriptableObject>("Save/SaveData");
        m_saveObject.datas = m_saveObject.datas.OrderByDescending(s => s.Score).ToList();
        if (m_saveObject.datas.Count > 10)
        {
             m_saveObject.datas.RemoveRange(10, m_saveObject.datas.Count - 10);
        }
        if (m_saveObject.datas.Count > 0)
        {
            for (int i = 0; i < m_saveObject.datas.Count; ++i)
            {
                Positions.text += (i + 1).ToString();
                Positions.text += "\n";
                HighestScores.text += m_saveObject.datas[i].Score.ToString();
                HighestScores.text += "\n";
                for (int a = 0; a < m_saveObject.datas[i].StarCount; ++a)
                {
                    Stars.text += "<sprite index= 0>";
                }
                Stars.text += "\n";
            }
        }
     }
}
