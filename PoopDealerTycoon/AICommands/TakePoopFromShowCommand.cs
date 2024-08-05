using UnityEngine;
using Chameleon.Game.ArcadeIdle.Units;
using Chameleon.Game.ArcadeIdle.Helpers;

namespace Chameleon.Game.ArcadeIdle.Commands
{
    public class TakePoopFromShowCommand : TakeCommand
    {

        protected override Vector3 GetTargetPositionWithType(PoopType poopType)
        {
            return ScenePointManager.instance.GetPoopShowPlacePositionByType(poopType);
        }

        protected override Transform GetLookTargetTransform()
        {
            return ScenePointManager.instance.GetPoopShowplaceLookTarget(_poopTypeToTake);
        }

        public override void SkipCommand()
        {
            CustomerUnit customerUnit = _targetAIMovementController.GetComponent<CustomerUnit>();
            customerUnit.RemoveNextWant();
            base.SkipCommand();
        }

        protected override void OnReachedDestination()
        {
            base.OnReachedDestination();
            _targetAIMovementController.GetComponent<Movement.CustomerAIController>().StartTimeoutRoutine();
        }

    }

}
