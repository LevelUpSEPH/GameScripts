using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetController : TargetControllerBase
{
    [SerializeField] private TargetScriptableObject _targetSO;
    [SerializeField] private GameObject _characterModel;
    [SerializeField] private GameObject _propModel;
    [SerializeField] private GameObject _caughtCharacterModel;

    private void Awake()
    {
        Initialize();
    }

    private void Start(){
        EnableHidingCharacterModel();
        _caughtCharacterModel.SetActive(false);
    }

    private void Initialize()
    {
        _maxCaughtPercentage = _targetSO.maxCaughtPercentage;
    }

    public override void StartGetCaught()
    {
        if(_isCaught)
            return;
        _isCaught = true;

        TryPlayCaughtParticle();
            
        EnableCaughtCharacterModel();
        StopAllCoroutines();
        HideCatchingUI();
        InvokeTargetGotCaught();
    }

    protected override void EnableShownCharacterModel()
    {
        _characterModel.SetActive(true);
        _propModel.SetActive(false);
    }
    
    protected override void EnableHidingCharacterModel()
    {
        _characterModel.SetActive(false);
        _propModel.SetActive(true);
    }

    protected override void EnableCaughtCharacterModel(){
        ResetScale();
        _caughtCharacterModel.SetActive(true);
        _caughtCharacterModel.transform.rotation = Quaternion.Euler(0,180,0);
        _characterModel.SetActive(false);
        _propModel.SetActive(false);
    }

    private void ResetScale()
    {
        transform.localScale = Vector3.one;
    }
    
}
