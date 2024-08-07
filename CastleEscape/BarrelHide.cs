using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarrelHide : MonoBehaviour
{
    public static event Action PlayerHid;

    private GameObject _playerRef;
    private PlayerMovement _playerMovement;
    private bool _isPlayerHiding;
    [SerializeField] private Transform _exitPosition;
    private bool _isOnCooldown = false;

    private bool _isEnteringOnCooldown = false;

    [SerializeField] private int _counterEndValue = 10;

    private bool _playerEntering;
    private float _enteringCounter;

    private bool _isCoroutineActive = false;

    private void Start(){
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = _playerRef.GetComponent<PlayerMovement>();
    }
    private void Update(){
        if(_isPlayerHiding)
            if(_playerMovement.GetIsMoving())
                Leave();
        if(!_isPlayerHiding)
            if(_enteringCounter >= _counterEndValue ){
                Hide();
            }
    }

    private void OnTriggerEnter(Collider other){
        if(_isEnteringOnCooldown)
            return;
        if(other.gameObject.CompareTag("Player")){
            _playerEntering = true;
            if(!_isCoroutineActive)
                StartCoroutine(HideCountdown());
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            _playerEntering = false;
        }
    }

    private IEnumerator HideCountdown(){
        while(_playerEntering){
            float tickInterval = 0.1f;
            yield return new WaitForSeconds(tickInterval);
            if(_enteringCounter < 10)
                _enteringCounter++;
        }
        while(!_playerEntering){
            float tickInterval = 0.1f;
            yield return new WaitForSeconds(tickInterval);
            if(_enteringCounter > 0)
                _enteringCounter--;
        }
    }

    private void Hide(){
        PlayerHid?.Invoke();

        _isOnCooldown = true;
        StartCoroutine(LeaveCooldown());

        PlayerController playerController = _playerRef.GetComponent<PlayerController>();
        playerController.HidePlayer();
        _isPlayerHiding = true;

        StopCoroutine(HideCountdown());
        _isCoroutineActive = false;
        _playerEntering = false;
        _enteringCounter = 0;
    }

    private void Leave(){
        if(_isOnCooldown)
            return;

        _playerRef.transform.position = _exitPosition.position;
        PlayerController playerController = _playerRef.GetComponent<PlayerController>();
        playerController.ShowPlayer();
        _isPlayerHiding = false;
        _isEnteringOnCooldown = true;
        StartCoroutine(ReEnterCooldown());
    }

    private IEnumerator LeaveCooldown(){
        float cooldownTime = 1f;
        yield return new WaitForSeconds(cooldownTime);

        _isOnCooldown = false;
    }

    private IEnumerator ReEnterCooldown(){
        float reEnterTime = 2f;
        yield return new WaitForSeconds(reEnterTime);
        
        _isEnteringOnCooldown = false;
    }

    public float GetEnteringCounterPercentage(){
        return _enteringCounter / _counterEndValue;
    }

    public bool GetIsPlayerHiding(){
        return _isPlayerHiding;
    }
}
