using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProgressBarViewBase : MonoBehaviour
{
    private bool _isOnCooldown = false;

    private bool _playerFilling = false;

    [SerializeField] private int _counterEndValue = 10;
    private float _enteringCounter = 0;

    private bool _isCoroutineActive = false;

    private void Update(){
        if(_enteringCounter >= _counterEndValue ){
            BarFilled();
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            _playerFilling = true;
            if(!_isCoroutineActive)
                StartCoroutine(ActivateBarCountdown(true)); //do something every n time function
        }
    }


    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            _playerFilling = false;
        }
    }

    private IEnumerator ActivateBarCountdown(bool isIncreasing){
        float limit;
        float counterEffect = 0;

        if (isIncreasing) {
            limit = _counterEndValue;
            counterEffect = 1;
        }            
        else            
        {
            limit = 0;
            counterEffect = -1;
        }

        while(_enteringCounter != limit) { // condition do do smth
            if(_isOnCooldown)
                continue;
            float tickInterval = 0.1f;
            yield return new WaitForSeconds(tickInterval);
            _enteringCounter += counterEffect;
        }



        while (_playerFilling){
            if(_isOnCooldown)
                continue;
            float tickInterval = 0.1f;
            yield return new WaitForSeconds(tickInterval);
            if(_enteringCounter < 10)
                _enteringCounter++;
        }
        while(!_playerFilling){
            if(_isOnCooldown)
                continue;
            float tickInterval = 0.1f;
            yield return new WaitForSeconds(tickInterval);
            if(_enteringCounter > 0)
                _enteringCounter--;
        }
    }

    private void BarFilled(){
        PlayBarFilledAction();
        _isOnCooldown = true;
        StartCoroutine(BarFillingCooldown());

        StopCoroutine(ActivateBarCountdown(false));

        _isCoroutineActive = false;

        _enteringCounter = 0;
    }

    protected virtual void PlayBarFilledAction(){
        // do smth
    }

    private IEnumerator BarFillingCooldown(){
        float barFillingCooldown = 2f;
        yield return new WaitForSeconds(barFillingCooldown);
        _isOnCooldown = false;
    }

    public float GetEnteringCounterPercentage(){
        return _enteringCounter / _counterEndValue;
    }

}
