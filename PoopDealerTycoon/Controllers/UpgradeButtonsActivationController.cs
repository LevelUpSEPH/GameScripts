using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Upgrade
{
    public class UpgradeButtonsActivationController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _upgradeButtons = new List<GameObject>();
        private bool _isInitialized = false;
        private int _activatedButtonIndex = 0;

        private void OnEnable()
        {
            if(!_isInitialized)
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            UnregisterEvents();
        }

        private void Initialize()
        {
            _isInitialized = true;
            _activatedButtonIndex = GetInitialButtonCountToActivate();
            ActivateButtonsUntilIndex(_activatedButtonIndex);
            RegisterEvents();
        }

        protected virtual void RegisterEvents()
        {

        }

        protected virtual void UnregisterEvents()
        {

        }

        protected void ActivateNextButton()
        {
            ActivateButtonAtIndex(_activatedButtonIndex);
            _activatedButtonIndex++;
        }

        protected virtual int GetInitialButtonCountToActivate()
        {
            return 0;
        }

        protected void ActivateButtonsUntilIndex(int index)
        {
            for(int i = 0; i < index; i++)
            {
                ActivateButtonAtIndex(i);
            }
        }

        protected void ActivateButtonAtIndex(int index)
        {
            _upgradeButtons[index].SetActive(true);
        }
    }
}