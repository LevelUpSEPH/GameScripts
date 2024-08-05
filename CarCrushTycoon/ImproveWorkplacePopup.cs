using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RocketNavigation.Models.Views.Popup
{
    public class ImproveWorkplacePopup : PopupView
    {
        public static event Action OpenedPopup;
        public static event Action ClosedPopup;
        
        [SerializeField] private Button _hireTabButton;
        [SerializeField] private Button _upgradeTabButton;

        [SerializeField] private GameObject _hireWindow;
        [SerializeField] private GameObject _upgradeWindow;

        protected override void Open(PopupParams popupParams)
        {
            SwitchToHireWindow();

            base.Open(popupParams);
        }

        protected override void OnOpened()
        {
            base.OnOpened();

            RegisterEvents();

            OpenedPopup?.Invoke();
        }

        protected override void OnClosed()
        {
            base.OnClosed();

            UnregisterEvents();

            ClosedPopup?.Invoke();
        }

        private void RegisterEvents()
        {
            _hireTabButton.onClick.AddListener(SwitchToHireWindow);
            _upgradeTabButton.onClick.AddListener(SwitchToUpgradeWindow);
        }

        private void UnregisterEvents()
        {
            _hireTabButton.onClick.RemoveListener(SwitchToHireWindow);
            _upgradeTabButton.onClick.RemoveListener(SwitchToUpgradeWindow);
        }

        private void SwitchToHireWindow()
        {
            _upgradeWindow.SetActive(false);
            _hireWindow.SetActive(true);
        }

        private void SwitchToUpgradeWindow()
        {
            _hireWindow.SetActive(false);
            _upgradeWindow.SetActive(true);
        }
    }

}
