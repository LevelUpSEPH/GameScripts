using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrelHideUI : MonoBehaviour
{
    private BarrelHide _barrelHide;
    [SerializeField] private Image _percentageImage;
    [SerializeField] private GameObject _exitDisplay;
    
    private void Awake(){
        _barrelHide = GetComponent<BarrelHide>();
    }
    private void Update(){
        UpdateVisual();
    }

    private void UpdateVisual(){
        if(_barrelHide.GetIsPlayerHiding()){
            _percentageImage.gameObject.SetActive(false);
            _exitDisplay.SetActive(true);
        }
            
        else{
            _percentageImage.gameObject.SetActive(true);
            _exitDisplay.SetActive(false);
        }
            
        float _percentageAmount = _barrelHide.GetEnteringCounterPercentage();
        _percentageImage.fillAmount = _percentageAmount;
    }
}
