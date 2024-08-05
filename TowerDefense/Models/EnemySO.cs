using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "ScriptableObjects/Enemy", order = 2)]
public class EnemySO : ScriptableObject {
    public int maxHealth;
    [System.NonSerialized] public int deathReward;
    public float movementSpeed;
    public float damage;
    public int littleMoneyReward;
    public int bigMoneyReward;
}
