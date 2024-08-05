using System;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Units
{
    public class AIWorkerUnit : WorkerUnit
    {
        public static event Action WorkerEnabled;
        [SerializeField] private PoopSet _targetSet;

        private void OnEnable()
        {
            WorkerEnabled?.Invoke();
        }
        protected override void SetTargetUpgradeSkill()
        {
            UpgradeSkillName upgradeSkillName = GetUpgradeSkillName();
            _targetUpgradeSkill = Helpers.UpgradeSkillDict.upgradableSkillsDict[upgradeSkillName];
        }

        public PoopSet GetTargetSet()
        {
            return _targetSet;
        }

        private UpgradeSkillName GetUpgradeSkillName()
        {
            string poopSetString = GetComponent<Units.AIWorkerUnit>().GetTargetSet().ToString();
            string skillName = "worker" + poopSetString + "Stack";
            return (UpgradeSkillName)System.Enum.Parse(typeof(UpgradeSkillName), skillName, true);
        }
    }
}