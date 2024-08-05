using UnityEngine;
using System;

namespace Chameleon.Game.ArcadeIdle
{
    public class CarDropZoneBehavior : MonoBehaviour
    {
        public event Action<bool> AvailabilityChanged;
        [SerializeField] private Transform _targetReferenceTransform;
        private CarController _parkedCar = null;
        private bool _isSpotFull => _parkedCar != null;

        private void OnTriggerEnter(Collider other)
        {
            if(_isSpotFull)
                return;
            if(other.CompareTag("Car"))
            {
                CarController triggeredCar = other.GetComponent<CarController>();
                OnTriggeredWithCar(triggeredCar);
            }
        }

        private void OnTriggeredWithCar(CarController triggeredCar)
        {
            SetParkedCar(triggeredCar);
            
            _parkedCar.ReleaseUnit();
        }

        private void SetParkedCar(CarController parkedCar)
        {
            _parkedCar = parkedCar;

            if(_parkedCar != null)
            {
                parkedCar.SetIsCarActive(false);
            }

            AvailabilityChanged?.Invoke(_parkedCar == null);
        }

        public bool TryGetParkedCar(out CarController carController)
        {
            if(_parkedCar != null)
            {
                carController = _parkedCar;
                return true;
            }
            carController = null;
            return false;
        }

        public void ClearZone()
        {
            SetParkedCar(null);
        }
    }
}