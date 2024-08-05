using Chameleon.Game.ArcadeIdle.Movement;
using System;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public interface ICommand
    {
        public void PlayCommand(BaseAIMovementController baseAI, Action onCompleteAction = null);

        public void PlayCommand(BaseAIMovementController baseAI, PoopType poopType, Action onCompleteAction = null, bool isSameCommandAsPrevious = false);

        public bool CanPlayCommand(PoopType poopType);
        public void CompleteCommand();

        public void SkipCommand();
    }
}