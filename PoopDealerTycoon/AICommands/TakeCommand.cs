using System;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;
using Chameleon.Game.ArcadeIdle.Helpers;
using Chameleon.Game.ArcadeIdle.Movement;

namespace Chameleon.Game.ArcadeIdle.Commands
{
    public class TakeCommand : AICommand
    {
        private bool _subscribedToComplete = false;
        private bool _startedWalk = false;
        protected PoopType _poopTypeToTake;
        public override void PlayCommand(BaseAIMovementController baseAI, PoopType poopType, Action onCompleteAction = null, bool isSameCommandAsPrevious = false)
        {
            base.PlayCommand(baseAI, poopType, onCompleteAction);
            if(poopType == PoopType.None)
            {
                SkipCommand();
                return;
            }
            if(!isSameCommandAsPrevious)
            {
                Vector3 targetPosition = GetTargetPositionWithType(poopType);
                MoveToPoopCollectionZone(targetPosition);
                if(targetPosition == Vector3.zero)
                {
                    SkipCommand();
                    return;
                }
            }
            else
                OnReachedDestination();
            
            _poopTypeToTake = poopType;
        }

        private void MoveToPoopCollectionZone(Vector3 targetPosition)
        {
            _startedWalk = true;
            _targetAIMovementController.MoveToPosition(targetPosition);
            _targetAIMovementController.ReachedDestination += OnReachedDestination;
        }

        protected virtual Vector3 GetTargetPositionWithType(PoopType poopType)
        {
            return Vector3.zero;
        }

        protected virtual void OnReachedDestination()
        {
            if(!_startedWalk)
            {
                TakePoop();
            }
            else
            {
                _targetAIMovementController.ReachedDestination -= OnReachedDestination;
                Transform targetTransform = GetLookTargetTransform();
                RotateAnimator.instance.RotateTowards(_targetAIMovementController.transform, targetTransform, .5f, TakePoop);
            }
        }

        protected virtual Transform GetLookTargetTransform()
        {
            return null;
        }

        private void TakePoop()
        {
            if(_subscribedToComplete)
                return;
            BaseUnit baseUnit = _targetAIMovementController.GetComponent<BaseUnit>();
            baseUnit.SetCanPickPoop(true);
            SubscribeToComplete();
        }

        protected virtual void SubscribeToComplete()
        {
            ArcadeIdle.Abstract.BaseUnit baseUnit = _targetAIMovementController.GetComponent<ArcadeIdle.Abstract.BaseUnit>();
            baseUnit.PickedPoop += OnPickPoopComplete;
            _subscribedToComplete = true;
        }

        protected virtual void  UnsubscribeFromComplete()
        {
            ArcadeIdle.Abstract.BaseUnit baseUnit = _targetAIMovementController.GetComponent<ArcadeIdle.Abstract.BaseUnit>();
            baseUnit.PickedPoop -= OnPickPoopComplete;
            _subscribedToComplete = false;
        }

        private void OnPickPoopComplete(PoopBase pickedPoop)
        {
            CompleteCommand();
        }

        public override void CompleteCommand()
        {
            _targetAIMovementController.ReachedDestination -= OnReachedDestination;
            BaseUnit baseUnit = _targetAIMovementController.GetComponent<BaseUnit>();
            UnsubscribeFromComplete();
            baseUnit.SetCanPickPoop(false);
            base.CompleteCommand();
        }
    }
}