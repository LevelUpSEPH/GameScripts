using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public abstract class OpenPopupTriggerBase : MonoBehaviour
    {
        protected bool _isPopupOpen = false;

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(_isPopupOpen)
                    return;
                OpenPopup();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                ClosePopup();
            }
        }

        protected abstract void OpenPopup();
        protected abstract void ClosePopup();

        protected abstract void RegisterEvents();
        
        protected abstract void UnregisterEvents();
    }
}