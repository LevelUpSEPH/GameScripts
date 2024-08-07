using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OpenDoorButton : MonoBehaviour
{
    public static event Action AnyDoorButtonPressed;

    [SerializeField] private DoorWithButton _targetDoorWithButton;
    [SerializeField] private GameObject _lockedButton;
    [SerializeField] private GameObject _unlockedButton;
    [SerializeField] private GameObject _latch;

    [SerializeField] private MeshRenderer _buttonMeshRenderer;
    [SerializeField] private Material _greenColoredMaterial;
    private Vector3 _latchStartingPos;

    private bool _buttonPressed = false;

    private void Start(){
        _latchStartingPos = _latch.transform.position;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            _latch.transform.position = new Vector3(_latchStartingPos.x, _latchStartingPos.y -0.1f, _latchStartingPos.z);
            if(_buttonPressed)
                return;
            AnyDoorButtonPressed?.Invoke();
            _buttonMeshRenderer.material = _greenColoredMaterial;
            _targetDoorWithButton.OpenDoorWithButton();
            _lockedButton.SetActive(false);
            _unlockedButton.SetActive(true);
            _buttonPressed = true;
        }
            
    }
    
    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player"))
            _latch.transform.position = _latchStartingPos;
    }

}
