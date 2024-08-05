using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathController : MonoBehaviour
{
    [SerializeField] private List<Transform> _aiPath = new List<Transform>();

    private void Awake(){
        foreach(Transform child in transform)
            _aiPath.Add(child);
    }

    public List<Transform> GetPath(){
        return _aiPath;
    }
}
