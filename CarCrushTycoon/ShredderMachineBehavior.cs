using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;
using DG.Tweening;
using System;

namespace Chameleon.Game.ArcadeIdle.Machine
{
    public class ShredderMachineBehavior : BaseCrushingMachine
    {
        [SerializeField] private ParticleSystem _tearingMetal;
        [SerializeField] private Transform _leftRoller;
        [SerializeField] private Transform _rightRoller;
        [SerializeField] private float _moveAmountToReachCar;
        
        private float _rollerInitialZPosition;
        private Quaternion _rollerInitialRotation;

        private float _durationToReachCar = 1;
        private float _durationToMakeCarFlat = 2.5f;

        private Coroutine _spinRollersRoutine;

        protected override void OnStart()
        {
            _rollerInitialZPosition = _rightRoller.localPosition.z;
            _rollerInitialRotation = _rightRoller.localRotation;
        }
        
        protected override void PlayDestroyAnimation(Action onCompletedDestroy)
        {
            MoveRollersIn();
            
            DOVirtual.DelayedCall(_durationToReachCar, () => {
                _spinRollersRoutine = StartCoroutine(SpinRollers());
                SetTearingMetalPlaying(true);
                MoveCarThroughRollers();
            });
            
            DOVirtual.DelayedCall(_durationToMakeCarFlat + _durationToReachCar + .5f, () =>
            {
                StopRollers();
                onCompletedDestroy();
            });
        }

        private void MoveRollersIn()
        {
            _leftRoller.DOLocalMoveZ(_leftRoller.localPosition.z + _moveAmountToReachCar, _durationToReachCar).SetEase(Ease.Linear);
            _rightRoller.DOLocalMoveZ(_rightRoller.localPosition.z - _moveAmountToReachCar, _durationToReachCar).SetEase(Ease.Linear);
        }

        private IEnumerator SpinRollers()
        {
            while(true)
            {
                yield return new WaitForSeconds(.025f);
                
                _leftRoller.Rotate(new Vector3(0, -5f, 0));
                _rightRoller.Rotate(new Vector3(0, 5f, 0));
            }
        }

        private void MoveCarThroughRollers()
        {
            _carToAnimate.DOLocalMoveZ(_carToAnimate.localPosition.z + 3f, _durationToMakeCarFlat).SetEase(Ease.Linear);
            _carToAnimate.DOScaleX(.2f, _durationToMakeCarFlat);
        }
        
        private void StopRollers()
        {
            StopCoroutine(_spinRollersRoutine);
            SetTearingMetalPlaying(false);

            ResetRollersRotation();
            MoveRollersOut();
        }

        private void ResetRollersRotation()
        {
            _rightRoller.localRotation = _rollerInitialRotation;
            _leftRoller.localRotation = _rollerInitialRotation;
        }

        private void MoveRollersOut()
        {
            _leftRoller.DOLocalMoveZ(-_rollerInitialZPosition, _durationToReachCar).SetEase(Ease.Linear);
            _rightRoller.DOLocalMoveZ(_rollerInitialZPosition, _durationToReachCar).SetEase(Ease.Linear);
        }

        private void SetTearingMetalPlaying(bool isPlaying)
        {
            if(isPlaying)
            {
                _tearingMetal.Play();
            }
            else
            {
                _tearingMetal.Stop();
            }
        }
    }
}