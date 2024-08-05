using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketNavigation;
using RocketNavigation.Models.Views.Popup;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle
{
    public class OpenImproveWorkplaceTrigger : OpenPopupTriggerBase
    {
        protected override void RegisterEvents()
        {
            ImproveWorkplacePopup.OpenedPopup += OnOpenedPopup;
            ImproveWorkplacePopup.ClosedPopup += OnClosedPopup;
        }

        protected override void UnregisterEvents()
        {
            ImproveWorkplacePopup.OpenedPopup -= OnOpenedPopup;
            ImproveWorkplacePopup.ClosedPopup -= OnClosedPopup;
        }

        protected override void OpenPopup()
        {
            Nav.Popup.Open("ImproveWorkplacePopup");
        }

        protected override void ClosePopup()
        {
            Nav.Popup.Close("ImproveWorkplacePopup");
        }

        private void OnOpenedPopup()
        {
            _isPopupOpen = true;
        }

        private void OnClosedPopup()
        {
            _isPopupOpen = false;
        }
    }
}