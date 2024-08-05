using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class CarDropZoneAnimator : ZoneAnimator
    {
        [SerializeField] private GameObject _dropCarText;
        private CarDropZoneBehavior _targetCarDropZone;

        protected override void Start()
        {
            base.Start();

            _targetCarDropZone = GetComponent<CarDropZoneBehavior>();

            RegisterEvents();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            _targetCarDropZone.AvailabilityChanged += OnZoneAvailabilityChanged;
        }

        private void UnregisterEvents()
        {   
            _targetCarDropZone.AvailabilityChanged -= OnZoneAvailabilityChanged;
        }

        private void OnZoneAvailabilityChanged(bool isAvailable)
        {
            if(isAvailable)
            {
                Animate();
            }
            else
            {
                StopAnimate();
            }

            SetDropCarTextActive(isAvailable);
            SetBordersActive(isAvailable);
        }

        private void SetDropCarTextActive(bool isActive)
        {
            _dropCarText.SetActive(isActive);
        }
    }
}