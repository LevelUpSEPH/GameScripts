using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _inGameCamera, _preGameCamera, _lampCamera, _endGameCamera;

    private List<GameObject> _cameraList = new List<GameObject>();

    private void OnEnable(){
        InLevelController.LevelStarted += OnLevelStarted;
        FinishTrigger.ParkourFinished += OnParkourFinished;
        LampBehaviour.LampTransferFinished += OnLampTransferFinished;
    }

    private void OnDisable(){
        InLevelController.LevelStarted -= OnLevelStarted;
        FinishTrigger.ParkourFinished -= OnParkourFinished;
        LampBehaviour.LampTransferFinished -= OnLampTransferFinished;
    }

    private void Start() {
        _cameraList.Add(_inGameCamera);
        _cameraList.Add(_preGameCamera);
        _cameraList.Add(_lampCamera);
        _cameraList.Add(_endGameCamera);
    }

    private void DisableCameras() {
        foreach (GameObject cam in _cameraList) {
            cam.SetActive(false);
        }
    }

    private void OnLevelStarted(){
        DisableCameras();
        _inGameCamera.SetActive(true);
    }

    private void OnParkourFinished(){
        DisableCameras();
        _lampCamera.SetActive(true);        
    }

    private void OnLampTransferFinished() {
        DisableCameras();
        _endGameCamera.SetActive(true);
    }
    
}
