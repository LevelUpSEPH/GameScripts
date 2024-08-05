using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LampBehaviour : MonoBehaviour
{
    [SerializeField] private Image _filler;

    private int _capacity = 300;
    private int _holdingXPAmount = 0;

    private int _transferRatio = 5;
    private PlayerController _player;

    public static event Action LampTransferFinished; 

    private void Start() {
        UpdateFiller();
        _player = FindObjectOfType<PlayerController>();

        FinishTrigger.ParkourFinished += StartTransferingAccuracyXP;
    }

    private void OnDisable() {
        FinishTrigger.ParkourFinished -= StartTransferingAccuracyXP;
    }

    private void StartTransferingAccuracyXP() {
        StartCoroutine(TransferAccuracyExp());
    }

    private IEnumerator TransferAccuracyExp() {
        while(_player.GetTotalExp() > 0) {
            _player.ChangeAccuracyExp(_transferRatio * -1);
            _holdingXPAmount += _transferRatio;
            UpdateFiller();
            yield return new WaitForSeconds(0.05f);
        }
            LampTransferFinished?.Invoke();                       
    }

    private void UpdateFiller() {
        _filler.fillAmount = CalculateFillerRatio();
    }

    private float CalculateFillerRatio() {
        return (float)_holdingXPAmount / (float)_capacity;
    }
}
