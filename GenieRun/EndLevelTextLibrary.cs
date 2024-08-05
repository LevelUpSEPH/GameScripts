using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EndLevelTextLibrary : Singleton<EndLevelTextLibrary> // This is a library for EndLevelTextLibrary
{
    [SerializeField] private List<EndLevelText> _EndLevelTexts = new List<EndLevelText>();
    private List<string> _pickedTexts = new List<string>();
    private List<Sprite> _pickedImages = new List<Sprite>();
    [SerializeField] private EndLevelText _kolpaList;
    private int _indicatorLevel = 0;
    private int _accuracyLevel = 0;
    private int _randomNumber = 0;

    private int _indicatorBorder = 100;

    public static event Action SetStatsComplete;

    private void OnEnable(){
        PlayerController.FortuneStatsAreReady += OnFortuneStatsAreReady;
    }

    private void OnDisable(){
        PlayerController.FortuneStatsAreReady -= OnFortuneStatsAreReady;
    }

    private void OnFortuneStatsAreReady(int indicatorPoint, int accuracyLevel){
        Debug.Log("Current outcome is : " + GetIndicatorLevel(indicatorPoint) + " HasEnoughAccuracy : " + accuracyLevel);
        SetStats(GetIndicatorLevel(indicatorPoint), accuracyLevel);
        SetStatsComplete?.Invoke();
    }

    private void SetStats(int indicatorLevel, int accuracyLevel){
        _indicatorLevel = indicatorLevel;
        _accuracyLevel = accuracyLevel;
    }

    public void SetResults(){
        if(_accuracyLevel == 0){
            Debug.Log("Not enough accuracy");
            _randomNumber = UnityEngine.Random.Range(0, _kolpaList.GetEndLevelText().Count);
            _pickedTexts.AddRange(_kolpaList.GetEndLevelText());
            _pickedImages.AddRange(_kolpaList.GetEndLevelImage());
            return;
        }
        foreach(EndLevelText endLevelText in _EndLevelTexts){
            if(endLevelText.GetIndicator() != _indicatorLevel)
                continue;
            _pickedTexts.AddRange(endLevelText.GetEndLevelText());
            _pickedImages.AddRange(endLevelText.GetEndLevelImage());
        }
        _randomNumber = UnityEngine.Random.Range(0, _pickedTexts.Count);
    }

    private int GetIndicatorLevel(int indicatorPoint) {
        if (indicatorPoint < _indicatorBorder)
            return 0;
        else
            return 1;
    }

    public string GetTextResult(){
        return _pickedTexts[_randomNumber];
    }

    public Sprite GetImageResult(){
        return _pickedImages[_randomNumber];
    }
}