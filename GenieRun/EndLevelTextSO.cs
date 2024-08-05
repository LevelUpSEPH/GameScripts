using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EndLevelTextSO", menuName = "ScriptableObjects/EndLevelTextLibrary", order = 1)]

public class EndLevelTextSO : ScriptableObject
{
    public List<string> endLevelText;
    public List<Sprite> endLevelImage;
    
}