using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
public class PlayerData : ScriptableObject
{
    public bool isMotorcycle;
    public bool isWinnedLevel;
    public float valua_audio;
    public int level_Winned = -1;
    public List<string> levelWinned;
    public int Sum_Time_Finish;
}
