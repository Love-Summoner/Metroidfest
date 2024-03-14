using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int progress;
    public float respawnx;
    public float respawny;

    public GameData()
    {
        progress = 0;
        respawnx = -9;
        respawny = -1;
    }
}
