using UnityEngine;


namespace Chameleon.Game.ArcadeIdle.Units
{
    public abstract class NeedsUI : MonoBehaviour
    {
        [SerializeField] private GameObject _visuals;

        protected virtual void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        protected abstract void RegisterEvents();

        protected abstract void UnregisterEvents();

        protected abstract void UpdateWantedImage();

        protected void SetVisualsActive(bool isVisualsActive)
        {
            _visuals.gameObject.SetActive(isVisualsActive);
        }
    }
}