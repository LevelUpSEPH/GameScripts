using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 3)]
public class WaveSO : ScriptableObject {

    public float waveTime;
    public List<string> enemyTags = new List<string>();

}