using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Chameleon.Game.ArcadeIdle
{
    public class BuildZoneUI : MonoBehaviour
    {
        [SerializeField] private BuildZone _targetBuildZone;
        [SerializeField] private Image _filledGround;
        [SerializeField] private TextMeshProUGUI _remainingMoneyText;

        private BuildableItem _targetBuildableItem;

        private void Start()
        {
            Initialize();

            RegisterEvents();

            if(_targetBuildableItem.IsInitialized)
                UpdateUIElements();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            _targetBuildableItem.BuildableItemInitialized += OnBuildableItemInitialized;
            _targetBuildableItem.RequiredMoneyChanged += UpdateUIElements;
        }

        private void UnregisterEvents()
        {
            _targetBuildableItem.BuildableItemInitialized -= OnBuildableItemInitialized;
            _targetBuildableItem.RequiredMoneyChanged -= UpdateUIElements;
        }

        private void Initialize()
        {
            _targetBuildableItem = _targetBuildZone.GetBuildableItem();
        }

        private void OnBuildableItemInitialized()
        {
            UpdateUIElements();
        }

        private void UpdateUIElements()
        {
            UpdateFilledGround();
            UpdateRemainingMoneyText();
        }

        private void UpdateFilledGround()
        {
            _filledGround.fillAmount = _targetBuildableItem.GetBuildPercentage();
        }

        private void UpdateRemainingMoneyText()
        {
            _remainingMoneyText.text = _targetBuildableItem.GetRequiredMoneyLeft().ToString();
        }
    }
}