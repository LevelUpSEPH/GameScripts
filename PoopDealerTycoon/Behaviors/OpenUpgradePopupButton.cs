using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RocketNavigation;
using Chameleon.Scripts.Models;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle.Upgrade
{
    public class OpenUpgradePopupButton : MonoBehaviour
    {
        [SerializeField] private Button _openUpgradePopupButton;

        private void Start()
        {
            _openUpgradePopupButton.onClick.AddListener(OpenUpgradePopup);
            if(PlayerData.Instance.IsPermanentCheckoutActive)
                SetUpgradeButtonActive(true);
            else
            {
                SetUpgradeButtonActive(false);
                PlayerData.Stats[StatKeys.IsPermanentCheckoutActive].Changed += OnPermanentCheckoutActived;
            }
        }

        private void OnDisable()
        {
            _openUpgradePopupButton.onClick.RemoveListener(OpenUpgradePopup);
            PlayerData.Stats[StatKeys.IsPermanentCheckoutActive].Changed -= OnPermanentCheckoutActived;
        }

        private void OnPermanentCheckoutActived()
        {
            PlayerData.Stats[StatKeys.IsPermanentCheckoutActive].Changed -= OnPermanentCheckoutActived;
            SetUpgradeButtonActive(true);
        }

        private void SetUpgradeButtonActive(bool isButtonActive)
        {
            _openUpgradePopupButton.gameObject.SetActive(isButtonActive);
        }

        private void OpenUpgradePopup()
        {
            Nav.Popup.Register(GameContentTypes.UpgradePopup);
        }
    }
}