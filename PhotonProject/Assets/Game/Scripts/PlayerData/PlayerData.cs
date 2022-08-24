using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public PlayerType PlayerType;
    public GameObject MyModel;
    public float WalkSpeed;
    public RuntimeAnimatorController AnimController;
    public RuntimeAnimatorController ShootController;
}
