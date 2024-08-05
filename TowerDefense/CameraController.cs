using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _castleCamera;
    [SerializeField] private GameObject _inGameCamera;
    [SerializeField] private GameObject _spawnerCamera;

    private void Start(){
        WaveController.WaveStarted += OnWaveStarted;
    }

    private void OnWaveStarted(){
        WaveController.WaveStarted -= OnWaveStarted;
        StartCoroutine(ActivateSpawnerCamera());
    }

    private IEnumerator ActivateSpawnerCamera(){
        TowerButtonsController.instance.SetTowerButtonsEnabled(false);
        if(GameStateManager.instance.GetGameState() == GameStateManager.GameState.PlacementState)
            PlacementController.instance.CancelPlacement();
        _inGameCamera.SetActive(false);
        _spawnerCamera.SetActive(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(ActivateCastleCamera());
    }

    private IEnumerator ActivateCastleCamera(){
        _castleCamera.SetActive(true);
        _spawnerCamera.SetActive(false);
        yield return new WaitForSeconds(3f);
        ActivateInGameCamera();
    }

    private void ActivateInGameCamera(){
        _inGameCamera.SetActive(true);
        _castleCamera.SetActive(false);
        TowerButtonsController.instance.SetTowerButtonsEnabled(true);
    }

}
