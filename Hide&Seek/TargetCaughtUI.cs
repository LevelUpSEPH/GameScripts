using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetCaughtUI : MonoBehaviour
{
    [SerializeField] private Image _circleImage;
    [SerializeField] private Image _icon;

    public void SetCaughtPercentage(float percentage){
        _circleImage.fillAmount =  percentage;
    }

    public void HideImage(){
        _circleImage.enabled = false;
        _icon.enabled = false;
    }

    public void ShowImage(){
        _circleImage.enabled = true;
        _icon.enabled = true;
    }
}
