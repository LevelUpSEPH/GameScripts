using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetImageBehaviour : MonoBehaviour
{

    private bool _isCaughtImage = false;
    [SerializeField] private Image _caughtImage;
    [SerializeField] private Image _hiddenImage;
    [SerializeField] private Image _cross;

    private void Start(){
        _caughtImage.enabled = false;
        _cross.enabled = false;
        _hiddenImage.enabled = true;
    }

    public void SwitchToCaught(){
        _caughtImage.enabled = true;
        _cross.enabled = true;
        _hiddenImage.enabled = false;

        _isCaughtImage = true;
    }

    public bool GetIsCaught(){
        return _isCaughtImage;
    }
}
