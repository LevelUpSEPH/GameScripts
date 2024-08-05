using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerUIController : MonoBehaviour 
{
    [SerializeField] private Image _filler;
    [SerializeField] private TextMeshProUGUI _tagText;
    [SerializeField] private GameObject _visualsParent;

    [SerializeField] private PlayerController _playerController;

    private List<string> _levelLabels = new List<string> { "Liar", "Moderate", "Bulls Eye" };
    
    private void Awake(){
        InLevelController.LevelStarted += EnableVisuals;
    }

    private void OnDisable(){
        InLevelController.LevelStarted -= EnableVisuals;
    }

    private void Update()
    {
        UpdateUI();
        if (Input.GetKeyDown("p"))
            Debug.Log(CalculateAccuracyLevelCompletionRatio());
    }

    private void EnableVisuals(){
        _visualsParent.SetActive(true);
    }

    private void UpdateUI(){ // REMAKE, ADD ACCURACY LEVELS
        _filler.fillAmount = (CalculateAccuracyLevelCompletionRatio());
        _tagText.text = _levelLabels[_playerController.GetAccuracyLevel() - 1];        
    }

    private float CalculateAccuracyLevelCompletionRatio() {
        return (float)_playerController.GetAccuracyExp() / (float)_playerController.GetRequiredExpToLevelUp();
    }
}