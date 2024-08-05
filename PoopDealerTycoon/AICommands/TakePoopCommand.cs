using UnityEngine;
using System.Collections;
using Chameleon.Game.ArcadeIdle.Helpers;
using Chameleon.Game.ArcadeIdle.Units;
using Chameleon.Game.ArcadeIdle.Movement;
//using RocketCoroutine;

namespace Chameleon.Game.ArcadeIdle.Commands
{
    public class TakePoopCommand : TakeCommand // for workers
    {
        protected override Vector3 GetTargetPositionWithType(PoopType poopType)
        {
            return ScenePointManager.instance.GetPoopCollectionZonePositionByType(poopType);
        }

        protected override Transform GetLookTargetTransform()
        {
            return ScenePointManager.instance.GetPoopStockPlaceLookTarget(_poopTypeToTake);
        }

        protected override void SubscribeToComplete()
        {
            base.SubscribeToComplete();
            RocketCoroutine.CoroutineController.StartCoroutine(SubscribeToSkipAfterDelay());
        }

        private IEnumerator SubscribeToSkipAfterDelay()
        {
            yield return new WaitForSeconds(1);
            WorkerUnit workerUnit = _targetAIMovementController.GetComponent<WorkerUnit>();
            workerUnit.FailedToPickPoop += SkipCommand;
        }

        public override void SkipCommand()
        {
            WorkerAIController workerAI = _targetAIMovementController.GetComponent<WorkerAIController>();
            workerAI.SkipCycle();
            base.SkipCommand();
        }

        protected override void UnsubscribeFromComplete()
        {
            base.UnsubscribeFromComplete();
            WorkerUnit workerUnit = _targetAIMovementController.GetComponent<WorkerUnit>();
            workerUnit.FailedToPickPoop -= SkipCommand;
        }

        public override void CompleteCommand()
        {
            base.CompleteCommand();
            RocketCoroutine.CoroutineController.StopCoroutine(SubscribeToSkipAfterDelay());
        }

    }
}