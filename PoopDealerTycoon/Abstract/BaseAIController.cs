using UnityEngine;
using Chameleon.Game.ArcadeIdle.Movement;
using System;
using Chameleon.Game.ArcadeIdle.Commands;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public class BaseAIController : MonoBehaviour
    {
        protected BaseAIMovementController _movementController;
        protected bool _isAvailableForCommands = true;

        protected virtual void Start()
        {
            _movementController = GetComponent<BaseAIMovementController>();
        }

        private void OnEnable()
        {
            ResetController();
        }

        private void Update()
        {
            HandleCommanding();
        }

        protected virtual void ResetController()
        {
            _isAvailableForCommands = true;
        }

        protected virtual void HandleCommanding(){}

        protected void GiveCommand(AICommand command, Action onCompleteAction = null)
        {
            command.PlayCommand(_movementController, onCompleteAction);
            _isAvailableForCommands = false;
        }

        protected void GiveCommandWithPoopTarget(AICommand command, PoopType poopType, Action onCompleteAction = null, bool isSameCommandAsPrevious = false)
        {
            command.PlayCommand(_movementController, poopType, onCompleteAction, isSameCommandAsPrevious);
        }

        protected virtual void ReleaseAvailibity()
        {
            _isAvailableForCommands = true;
        }
    }
}