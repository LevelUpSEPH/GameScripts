using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Chameleon.Game.ArcadeIdle
{
    public class SpecialCustomerTimerUI : MonoBehaviour
    {
        [SerializeField] private GameObject _visuals;
        [SerializeField] private Image _clockFillerImage;
        [SerializeField] private TextMeshProUGUI _remainingTimeText;
        private SpecialCustomerBehaviour _specialCustomerBehaviour;
        private bool _isSpecialCustomerActive = false;

        private void Start()
        {
            SpecialCustomerBehaviour.SpecialCustomerSpawned += OnSpecialCustomerSpawned;
            SpecialCustomerBehaviour.SpecialCustomerLeft += OnSpecialCustomerLeft;
        }

        private void OnDisable()
        {
            SpecialCustomerBehaviour.SpecialCustomerSpawned -= OnSpecialCustomerSpawned;
            SpecialCustomerBehaviour.SpecialCustomerLeft -= OnSpecialCustomerLeft;
        }

        private void Update()
        {
            if(_isSpecialCustomerActive)
            {
                UpdateUI();
            }
        }

        private void OnSpecialCustomerSpawned(SpecialCustomerBehaviour specialCustomer)
        {
            _specialCustomerBehaviour = specialCustomer;
            StartUpdatingTimer();
            SetVisualsActive(true);
        }

        private void OnSpecialCustomerLeft()
        {
            StopUpdatingTimer();
            _isSpecialCustomerActive = false;
            SetVisualsActive(false);
        }

        private void StartUpdatingTimer()
        {
            _isSpecialCustomerActive = true;
        }

        private void StopUpdatingTimer()
        {
            _isSpecialCustomerActive = false;
        }

        private void SetVisualsActive(bool isVisualsActive)
        {
            _visuals.SetActive(isVisualsActive);
        }

        private void UpdateUI()
        {
            _remainingTimeText.text = Helpers.TimeConverter.ConvertSecondsToMinutes(_specialCustomerBehaviour.GetRemainingTime());
            _clockFillerImage.fillAmount = _specialCustomerBehaviour.GetRemainingTimePercentage();
        }
    }
}
