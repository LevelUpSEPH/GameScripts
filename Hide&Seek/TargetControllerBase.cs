using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TargetControllerBase : MonoBehaviour, IHider
{
    public static event Action TargetGotCaught;

    [SerializeField] private TargetCaughtUI _targetCaughtUI;
    [SerializeField] private ParticleSystem _caughtParticle;
    private Coroutine _isHiddenCor;
    private Coroutine _isBeingCaughtCor;
    protected bool _isCaught = false;
    private float _caughtPercentage = 0f;
    protected float _maxCaughtPercentage = 100;
    private bool _isShown = false;
    private bool _isHidden = true;

    protected void ShowCatchingUI()
    {
        _targetCaughtUI.ShowImage();
    }

    protected void HideCatchingUI()
    {
        _targetCaughtUI.HideImage();
    }

    protected void InvokeTargetGotCaught()
    {
        TargetGotCaught?.Invoke();
    }

    public void ShowTarget(float incrementPerTick){
        if(_isShown)
            return;
        if(_isCaught)
            return;
        _isShown = true;
        if(_isHiddenCor != null)
            StopCoroutine(_isHiddenCor);
        _isBeingCaughtCor = StartCoroutine(IsBeingCaughtCor(incrementPerTick));
        EnableShownCharacterModel();
        ShowCatchingUI();
        _isHidden = false;
    }

    public void HideTarget(){
        if(_isHidden)
            return;
        if(_isCaught)
            return;
        _isHidden = true;
        if(_isBeingCaughtCor != null)
            StopCoroutine(_isBeingCaughtCor);
        _isHiddenCor = StartCoroutine(IsHiddenCor());
        EnableHidingCharacterModel();
        HideCatchingUI();
        _isShown = false;
    }

    private void IncrementCaughtPercentage(float incrementPerTick){
        _caughtPercentage += Time.deltaTime * incrementPerTick;
        _targetCaughtUI.SetCaughtPercentage(_caughtPercentage / _maxCaughtPercentage);
        if(_caughtPercentage > _maxCaughtPercentage)
            _caughtPercentage = _maxCaughtPercentage;
    }

    private void DecrementCaughtPercentage(){
        _caughtPercentage -= Time.deltaTime * 10f;
        _targetCaughtUI.SetCaughtPercentage(_caughtPercentage / _maxCaughtPercentage);
        if(_caughtPercentage < 0)
            _caughtPercentage = 0;
    }

    private IEnumerator IsBeingCaughtCor(float incrementPerTick){
        while(true){
            yield return null;
            IncrementCaughtPercentage(incrementPerTick);
        }
    }

    private IEnumerator IsHiddenCor(){
        while(true){
            yield return null;
            if(_caughtPercentage <= 0)
                break;
            DecrementCaughtPercentage();
        }
    }

    protected void TryPlayCaughtParticle()
    {
        if(_caughtParticle != null)
            _caughtParticle.Play();
    }

    public float GetCaughtPercentage(){
        return _caughtPercentage / _maxCaughtPercentage;
    }

    public bool GetIsCaught()
    {
        return _isCaught;
    }

    protected abstract void EnableCaughtCharacterModel();
    protected abstract void EnableHidingCharacterModel();
    protected abstract void EnableShownCharacterModel();
    public abstract void StartGetCaught();
    
}
