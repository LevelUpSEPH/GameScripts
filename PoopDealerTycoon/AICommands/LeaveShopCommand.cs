using System;
using System.Collections;
using System.Collections.Generic;
using Chameleon.Game.ArcadeIdle.Movement;
using Chameleon.Game.ArcadeIdle.Helpers;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Commands
{
    public class LeaveShopCommand : AICommand
    {
        public override void PlayCommand(BaseAIMovementController baseAI, Action onCompleteAction = null)
        {
            base.PlayCommand(baseAI, onCompleteAction);
            Vector3 targetLeavePosition = ScenePointManager.instance.GetRandomPositionToLeave();
            _targetAIMovementController.MoveToPosition(targetLeavePosition);
            _targetAIMovementController.ReachedDestination += OnReachedLeavePosition;
        }

        private void OnReachedLeavePosition()
        {
            _targetAIMovementController.ReachedDestination -= OnReachedLeavePosition;
            ObjectDisabler.DisableObjectWithDelay(_targetAIMovementController.gameObject, 2f);
            CompleteCommand();
        }
    }
}