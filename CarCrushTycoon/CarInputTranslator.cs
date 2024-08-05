using System.Collections;
using System.Collections.Generic;
using Chameleon.Game.ArcadeIdle.Abstract;
using UnityEngine;
using DavidJalbert.TinyCarControllerAdvance;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class CarInputTranslator : MonoBehaviour
    {
        [SerializeField] private float _maxSteeringAngle = 30;
        [SerializeField] private TCCAPlayer _targetCarController = null;
        private BaseMovementController _targetMovementController = null;
        private bool _isMovementActive => _targetMovementController != null;

        private bool _isOnForwardMode = true;

        private void Update()
        {
            if(!_isMovementActive) // dont do this sequence non-stop on update
            {
                _targetCarController.setMotor(0);
                _targetCarController.setSteering(0);
                _targetCarController.setHandbrake(true);
                return;
            }
            HandleMovement();
            HandleRotation();
        }

        public void SetTransform(Vector3 targetPos, Quaternion targetRot)
        {
            _targetCarController.immobilize();
            
            SetPosition(targetPos);
            SetRotation(targetRot);
        }

        private void SetPosition(Vector3 targetPos) // car has to be inactive
        {
            _targetCarController.setPosition(targetPos);
        }

        private void SetRotation(Quaternion targetRot)
        {
            _targetCarController.setRotation(targetRot);
        }

        private void HandleMovement()
        {
            float motorDelta = 0;
            float inputStrength = _targetMovementController.GetMovementVector().magnitude;
            if(inputStrength > 0)
            {
                _targetCarController.setHandbrake(false);

                if(_isOnForwardMode)
                {
                    motorDelta = 1;
                }
                else
                {
                    motorDelta = -1;
                }

                motorDelta *= inputStrength;
            }
            else
            {
                _targetCarController.setHandbrake(true);
            }
            _targetCarController.setMotor(motorDelta);
        }

        private void HandleRotation()
        {
            float steeringDelta = 0;

            if(_targetMovementController.GetMovementVector().magnitude > 0)
            {
                Transform carBody = _targetCarController.getCarBody().transform;
                Vector3 inputDirection = new Vector3(_targetMovementController.GetMovementVector().x, 0, _targetMovementController.GetMovementVector().z);
                float normalizedAngleBetween;

                normalizedAngleBetween = Vector3.SignedAngle(carBody.forward, inputDirection, Vector3.up) / _maxSteeringAngle;
                
                steeringDelta = Mathf.Clamp(normalizedAngleBetween, -1, 1);
            }
            else
                steeringDelta = 0;

            _targetCarController.setSteering(steeringDelta);
        }

        private void OnExitCar()
        {
            
        }

        public void SetTargetUnit(BaseUnitController targetUnit)
        {
            if(targetUnit != null)
                _targetMovementController = targetUnit.GetMovementController();
            else
                _targetMovementController = null;
        }

        public bool GetIsOnForward()
        {
            return _isOnForwardMode;
        }
        
        public void ToggleIsOnForward()
        {
            _isOnForwardMode = !_isOnForwardMode;
        }

        public void SetIsOnForward(bool isOnForward)
        {
            _isOnForwardMode = isOnForward;
        }

        public void DisableCar()
        {
            _targetCarController.gameObject.SetActive(false);
        }

        public void SetWheelStats(float maxSteeringAngle, float maxSpeed, float acceleration)
        {
            _targetCarController.SetWheelStats(maxSteeringAngle, maxSpeed, acceleration);
        }
        
    }
}