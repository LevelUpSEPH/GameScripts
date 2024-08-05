using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelText : MonoBehaviour
{
    [SerializeField] private EndLevelTextSO _endLevelTextSO;
    private List<string> endLevelText;
    [SerializeField] private int _indicator = 0;
    private List<Sprite> endLevelImage;
    void Start()
    {
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetStats(){
        endLevelText = _endLevelTextSO.endLevelText;
        endLevelImage = _endLevelTextSO.endLevelImage;
    }

    public int GetIndicator(){
        return _indicator;
    }

    public List<string> GetEndLevelText(){
        return endLevelText;
    }

    public List<Sprite> GetEndLevelImage(){
        return endLevelImage;
    }
    
}
