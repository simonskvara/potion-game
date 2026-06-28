using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int Version = 1;
    public bool IsTutorialCompleted = false;
    public List<string> UnlockedPotions = new List<string>();
}
