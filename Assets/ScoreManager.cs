
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
    public TMP_SpriteAsset Star;
    SaveObj m_saveObject;
    PlayerStats m_player;
    /*
    What needs to be saved
    ---------------------------
    Score
    Player Lives // To check if they had died
    Big star was collected
         
         
    */

    void Start()
    {
    
    }
    void Update()
    {
        
    }

    public void SetDataToSave()
    {
        m_player = GameObject.Find("Player").GetComponent<PlayerStats>();
        _data.Score = m_player.GetScore(); 

        SaveToFile();
    }
  public void SaveToFile()
    {
        if(File.Exists("Assets/Resources/Save/SaveData.asset"))
        {
            m_saveObject = (SaveObj)Resources.Load<ScriptableObject>("Save/SaveData");
            m_saveObject.datas.Add(_data);
        }
    }
    public  void LoadIntoLeaderBoard()
    {
        Positions.text = "";
        HighestScores.text = "";
        Stars.text = "";
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
