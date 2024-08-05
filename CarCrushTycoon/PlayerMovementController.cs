using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle
{
    public class PlayerMovementController : BaseMovementController
    {
        private float _rotationSpeed = 6f;

        private FloatingJoystick joystick;
        private CharacterController _characterController;

        private void Awake()
        {
            Initiliaze();
        }

        private void Update()
        {
            MoveWithRotation();
        }

        protected override void Initiliaze()
        {
            base.Initiliaze();

            if (TryGetComponent<CharacterController>(out CharacterController characterController))
                _characterController = characterController;
            else
                Debug.LogError("No CharacterController Found!!");

            joystick = InputController.instance.GetFloatingJoystick();
        }

        private void MoveWithRotation()
        {
            float inputX = joystick.Horizontal;
            float inputZ = joystick.Vertical;
            
            Vector3 moveDir = new Vector3(inputX, 0, inputZ);

            if (Mathf.Abs(inputX) > 0 || Mathf.Abs(inputZ) > 0)
            {
                _isMoving = true;
                SetRunParticlePlaying(true);
            }
                
            else
            {
                _isMoving = false;
                SetRunParticlePlaying(false);
            }
            
            if(CanMove(moveDir))
            {
                _characterController.SimpleMove(moveDir * _speed);
                Rotate(moveDir);
                SetRunAnimationPlaying(true);
            }
            else
                SetRunAnimationPlaying(false);
        }

        private void Rotate(Vector3 targetDir)
        {
            if (targetDir == Vector3.zero)
                return;

            Quaternion rotation = Quaternion.Lerp(_model.rotation, Quaternion.LookRotation(targetDir), _rotationSpeed * targetDir.magnitude * Time.deltaTime);
            _model.rotation = rotation;
        }

        public override Vector3 GetMovementVector()
        {
            return new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        }
    }
}