using Chameleon.Game.ArcadeIdle.Abstract;
using Chameleon.Game.ArcadeIdle.Units;
using Chameleon.Game.ArcadeIdle.Helpers;
using Chameleon.Game.ArcadeIdle.Commands;
using UnityEngine;
using System.Collections;

namespace Chameleon.Game.ArcadeIdle.Movement
{
    public class CustomerAIController : BaseAIController
    {
        [SerializeField] private float _patienceTime = 20f;
        private CustomerUnit _customerUnit;
        private PoopType _lastPoopType = PoopType.None;
        private Coroutine _timeoutCoroutine;
        private bool _isCarryingAnyPoop = false;
        private bool _isCommandingActive = false;
        private bool _isTimeoutRoutineActive = false;

        protected override void Start()
        {
            base.Start();
            _customerUnit = GetComponent<CustomerUnit>();
        }

        private void OnDisable()
        {
            if(_timeoutCoroutine != null)
                StopCoroutine(_timeoutCoroutine);
        }

        protected override void HandleCommanding()
        {
            if(_isAvailableForCommands && _isCommandingActive)
            {
                _isAvailableForCommands = false;
                PoopType poopType = _customerUnit.GetNextWant();
                if(poopType == PoopType.None)
                {
                    GotoCheckout();
                }
                else
                {
                    AICommand aiCommand = AICommandsByType.GetAICommandByType(AICommandType.TakePoopFromShow);
                    GiveCommandWithPoopTarget(aiCommand, poopType, ReleaseAvailibity, poopType == _lastPoopType);
                    _lastPoopType = poopType;   
                }
            }
        }

        private void GotoCheckout()
        {
            AICommand aiCommand = AICommandsByType.GetAICommandByType(AICommandType.GotoCheckout);
            GiveCommand(aiCommand, LeaveShop);
        }

        private void LeaveShop()
        {
            AICommand aiCommand = AICommandsByType.GetAICommandByType(AICommandType.LeaveShop);
            GiveCommand(aiCommand);
            _customerUnit.Leave();
        }

        protected override void ResetController()
        {
            base.ResetController();
            _lastPoopType = PoopType.None;

            _isCarryingAnyPoop = false;
            _isCommandingActive = true;

            if(_timeoutCoroutine != null)
            {
                StopCoroutine(_timeoutCoroutine);    
            }

            _isTimeoutRoutineActive = false;
        }

        protected override void ReleaseAvailibity()
        {
            _isCarryingAnyPoop = true;
            base.ReleaseAvailibity();
            if(_timeoutCoroutine != null)
                StopCoroutine(_timeoutCoroutine);
            _isTimeoutRoutineActive = false;
        }

        public void StartTimeoutRoutine()
        {
            if(!_isTimeoutRoutineActive)
            {
                _timeoutCoroutine = StartCoroutine(TimeoutCoroutine());
                _isTimeoutRoutineActive = true;
            }
        }

        private IEnumerator TimeoutCoroutine()
        {
            yield return new WaitForSeconds(_patienceTime);
            if(isActiveAndEnabled)
            {
                if(_isCarryingAnyPoop)
                    GotoCheckout();
                else
                    LeaveShop();
                _customerUnit.ClearWants();
                _isCommandingActive = false;
            }
        }
    }
}