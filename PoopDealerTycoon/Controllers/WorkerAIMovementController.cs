using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Upgrade;

namespace Chameleon.Game.ArcadeIdle.Movement
{
    public class WorkerAIMovementController : BaseAIMovementController
    {
        private float _movementSpeed;
        private float _baseSpeed;
        private UpgradeSkill _targetUpgradeSkill;

        protected override void Initialize()
        {
            base.Initialize();

            
            UpgradeSkillName upgradeSkillName = GetUpgradeSkillName();

            _targetUpgradeSkill = Helpers.UpgradeSkillDict.upgradableSkillsDict[upgradeSkillName];
        }
        
        private void Start()
        {
            _baseSpeed = _agent.speed;
            UpdateMovementSpeed();
            _targetUpgradeSkill.UpgradeEvent += UpdateMovementSpeed;
        }
        
        private void OnDisable()
        {
            _targetUpgradeSkill.UpgradeEvent -= UpdateMovementSpeed;
        }

        private void UpdateMovementSpeed()
        {
            _movementSpeed = _baseSpeed * _targetUpgradeSkill.GetCurrentLevelCoef();
            _agent.speed = _movementSpeed;
        }

        private UpgradeSkillName GetUpgradeSkillName()
        {
            string poopSetString = GetComponent<Units.AIWorkerUnit>().GetTargetSet().ToString();
            string skillName = "worker" + poopSetString + "Speed";
            return (UpgradeSkillName)System.Enum.Parse(typeof(UpgradeSkillName), skillName, true);
        }
    }
}