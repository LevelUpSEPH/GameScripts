using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Upgrade
{
    public class UnitUpgradeButtonActivationController : UpgradeButtonsActivationController
    {
        protected override int GetInitialButtonCountToActivate()
        {
            int activeWorkerCountInScene = FindActiveWorkerCountInScene();
            return activeWorkerCountInScene;
        }

        private int FindActiveWorkerCountInScene()
        {
            int activeWorkerCountInScene = 0;
            GameObject[] workers = GameObject.FindGameObjectsWithTag("Worker");
            foreach(GameObject worker in workers)
            {
                if(worker.activeInHierarchy)
                {
                    activeWorkerCountInScene++;
                }
            }
            return activeWorkerCountInScene;
        }

        protected override void RegisterEvents()
        {
            Chameleon.Game.ArcadeIdle.Units.AIWorkerUnit.WorkerEnabled += ActivateNextButton;
        }

        protected override void UnregisterEvents()
        {
            Chameleon.Game.ArcadeIdle.Units.AIWorkerUnit.WorkerEnabled -= ActivateNextButton;
        }
    }
}