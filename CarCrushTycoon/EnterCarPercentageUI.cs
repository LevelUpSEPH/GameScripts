using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chameleon.Game.ArcadeIdle.Unit;

namespace Chameleon.Game.ArcadeIdle
{
    public class EnterCarPercentageUI : MonoBehaviour
    {
        [SerializeField] private PlayerUnitController _targetPlayer;
        [SerializeField] private GameObject _visuals;
        [SerializeField] private Image _enterPercentageFiller;

        private void Start()
        {
            SetVisualsActive(false);

            RegisterEvents();
        }
        
        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            _targetPlayer.StartedEnteringCar += OnStartedEnteringCar;
            _targetPlayer.EnteredCar += OnEnteredCar;
            _targetPlayer.StoppedEnteringCar += OnStoppedEnteringCar;
            _targetPlayer.EnterCarPercentageChanged += OnEnterCarPercentageChanged;
        }

        private void UnregisterEvents()
        {
            _targetPlayer.StartedEnteringCar -= OnStartedEnteringCar;
            _targetPlayer.EnteredCar -= OnEnteredCar;
            _targetPlayer.StoppedEnteringCar -= OnStoppedEnteringCar;
            _targetPlayer.EnterCarPercentageChanged -= OnEnterCarPercentageChanged;
        }

        private void OnStartedEnteringCar(CarController enteringCar)
        {
            transform.position = enteringCar.GetEnterPercentageTargetTransform().position;
            SetVisualsActive(true);
            UpdateFillAmount();
        }

        private void OnEnteredCar(CarController unused)
        {
            SetVisualsActive(false);
        }

        private void OnStoppedEnteringCar()
        {
            SetVisualsActive(false);
        }

        private void OnEnterCarPercentageChanged()
        {
            UpdateFillAmount();
        }

        private void SetVisualsActive(bool isActive)
        {
            _visuals.SetActive(isActive);
        }
        
        private void UpdateFillAmount()
        {
            float enterPercentage = _targetPlayer.GetEnterCarPercentage();
            _enterPercentageFiller.fillAmount = enterPercentage;
        }
    }
}