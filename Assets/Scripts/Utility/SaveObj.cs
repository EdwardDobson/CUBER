﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SaveData", menuName = "SaveData", order = 1)]
[System.Serializable]
public class SaveObj : ScriptableObject
{
    public List<SaveData> datas = new List<SaveData>();
}
