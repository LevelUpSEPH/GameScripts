using UnityEngine;
using RocketNavigation;
using Chameleon.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle.Upgrade
{
    public class OpenUpgradePopupTrigger : MonoBehaviour
    {
        private bool _hasPlayerTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(!_hasPlayerTriggered)
                {
                    _hasPlayerTriggered = true;
                    OpenUpgradePopup();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                _hasPlayerTriggered = false;
            }
        }

        private void OpenUpgradePopup()
        {
            Nav.Popup.Register(GameContentTypes.UpgradePopup);
        }
    }
}