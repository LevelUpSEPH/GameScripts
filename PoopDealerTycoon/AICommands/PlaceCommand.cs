using Chameleon.Game.ArcadeIdle.Units;
using Chameleon.Game.ArcadeIdle.Movement;
using Chameleon.Game.ArcadeIdle.Helpers;
using System;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Commands
{
    public class PlaceCommand : AICommand
    {
        protected bool _startedWalk = false;
        protected PoopType _poopTypeToTake;
        private bool _subscribedToComplete = false;

        public override void PlayCommand(BaseAIMovementController baseAI, PoopType poopType, Action onCompleteAction = null, bool isSameCommandAsPrevious = false)
        {
            base.PlayCommand(baseAI, poopType, onCompleteAction);
            MoveToPlacementPosition(poopType);
            _poopTypeToTake = poopType;

            if(!isSameCommandAsPrevious)
                MoveToPlacementPosition(poopType);
            else
                OnReachedPosition();
        }

        private void MoveToPlacementPosition(PoopType poopType)
        {
            _startedWalk = true;
            Vector3 targetPoint = GetTargetPositionWithType(poopType);
            _targetAIMovementController.MoveToPosition(targetPoint);
            _targetAIMovementController.ReachedDestination += OnReachedPosition;
        }

        protected virtual Vector3 GetTargetPositionWithType(PoopType poopType)
        {
            return Vector3.zero;
        }

        private void OnReachedPosition()
        {
            if(!_startedWalk)
            {
                PlacePoop();
            }
            else
            {
                _targetAIMovementController.ReachedDestination -= OnReachedPosition;
                Transform targetTransform = GetLookTargetTransform();
                RotateAnimator.instance.RotateTowards(_targetAIMovementController.transform, targetTransform, .5f, PlacePoop);
            }
        }

        private void PlacePoop()
        {
            if(_subscribedToComplete)
            {
                return;
            }
            WorkerUnit workerUnit = _targetAIMovementController.GetComponent<WorkerUnit>();
            workerUnit.DroppedPoop += CompleteCommand;
            workerUnit.DisabledPoop += CompleteCommand;
            _subscribedToComplete = true;
            workerUnit.SetCanDropPoop(true);
        }

        public override void CompleteCommand()
        {
            WorkerUnit workerUnit = _targetAIMovementController.GetComponent<WorkerUnit>();
            workerUnit.DroppedPoop -= CompleteCommand;
            workerUnit.DisabledPoop -= CompleteCommand;
            _subscribedToComplete = false;
            workerUnit.SetCanDropPoop(false);
            base.CompleteCommand();
        }

        protected virtual Transform GetLookTargetTransform()
        {
            return null;
        }
    }
}