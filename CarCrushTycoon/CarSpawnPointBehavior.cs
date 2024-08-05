using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class CarSpawnPointBehavior : MonoBehaviour
    {
        public event Action<CarSpawnPointBehavior> CarLeftPoint;

        [SerializeField] private Transform _carTargetTransform;
        private CarController _carInsidePoint = null;
        public bool HasCarInsidePoint => _carInsidePoint != null;

        private void OnTriggerExit(Collider other)
        {
            if(!HasCarInsidePoint)
                return;

            if(other.CompareTag("Car"))
            {
                CarController leftCarController = other.GetComponent<CarController>();
                if(leftCarController == _carInsidePoint)
                {
                    OnCarLeftPoint();
                }
            }
        }
        
        private void OnCarLeftPoint()
        {
            SetCarInsidePoint(null);

            CarLeftPoint?.Invoke(this);
        }

        public void SetCarInsidePoint(CarController carInsidePoint)
        {
            _carInsidePoint = carInsidePoint;
        }

        public Transform GetCarTargetTransform()
        {
            return _carTargetTransform;
        }

        public CarController GetCarInsidePoint()
        {
            return _carInsidePoint;
        }

        public void UnlockCar()
        {
            _carInsidePoint.SetIsCarActive(true);
        }
    }
}