using RocketNavigation;
using RocketNavigation.Models.Views.Popup;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle
{
    public class OpenCarGalleryPopupTrigger : OpenPopupTriggerBase
    {
        [SerializeField] private string _galleryPopupName;

        protected override void RegisterEvents()
        {
            BuyCarPopup.OpenedBuyCarPopup += OnOpenedPopup;
            BuyCarPopup.ClosedBuyCarPopup += OnClosedPopup;
        }

        protected override void UnregisterEvents()
        {
            BuyCarPopup.OpenedBuyCarPopup -= OnOpenedPopup;
            BuyCarPopup.ClosedBuyCarPopup -= OnClosedPopup;
        }

        protected override void OpenPopup()
        {
            Nav.Popup.Open(_galleryPopupName);
        }

        protected override void ClosePopup()
        {
            Nav.Popup.Close(_galleryPopupName);
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