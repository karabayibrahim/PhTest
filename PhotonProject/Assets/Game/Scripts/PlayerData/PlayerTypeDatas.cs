using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTypeData", menuName = "ScriptableObjects/PlayerTypeData", order = 1)]
public class PlayerTypeDatas : ScriptableObject
{
    public List<PlayerData> PlayerDatas = new List<PlayerData>();
}
