using Chameleon.Game.ArcadeIdle.Helpers;
using Chameleon.Game.ArcadeIdle.Abstract;
using System;
using Chameleon.Game.ArcadeIdle.Movement;


namespace Chameleon.Game.ArcadeIdle.Commands
{
    public class AICommand : ICommand
    {
        private event Action _onCompleteAction = null;
        protected BaseAIMovementController _targetAIMovementController;

        public virtual void PlayCommand(BaseAIMovementController baseAI, PoopType poopType, Action onCompleteAction = null, bool isSameCommandAsPrevious = false)
        {
            _targetAIMovementController = baseAI;
            _onCompleteAction = onCompleteAction;

            if(!CanPlayCommand(poopType))
            {
                SkipCommand();
            }
        }

        public virtual void PlayCommand(BaseAIMovementController baseAI, Action onCompleteAction = null)
        {
            _targetAIMovementController = baseAI;
            _onCompleteAction = onCompleteAction;
        }
        
        public virtual bool CanPlayCommand(PoopType poopType)
        {
            if(PoopTypeActivityController.instance.GetIsPoopTypeActive(poopType))
                return true;
            return false;
        }

        public virtual void SkipCommand()
        {
            CompleteCommand();
        }

        public virtual void CompleteCommand()
        {
            if(_onCompleteAction != null)
                _onCompleteAction?.Invoke();
        }
    }
}
