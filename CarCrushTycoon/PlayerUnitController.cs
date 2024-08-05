using System;
using System.Collections;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;
using Chameleon.Game.ArcadeIdle.Helpers;
using PlayerData = Game.Scripts.Models.PlayerData;

namespace Chameleon.Game.ArcadeIdle.Unit
{
    public class PlayerUnitController : BaseUnitController
    {
        public event Action<CarController> StartedEnteringCar;
        public event Action StoppedEnteringCar;
        public event Action<CarController> EnteredCar;
        public event Action LeftCar;
        public event Action EnterCarPercentageChanged;

        private CarController _enteringCar;
        
        private Coroutine _enterCarRoutine = null;
        private float _enterCarPercentage = 0;

        private bool _isBuilding = false;
        private Coroutine _buildingRoutine = null;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if(other.CompareTag("BuildZone"))
            {
                BuildZone buildZone = other.GetComponent<BuildZone>();
                if(_isBuilding)
                {
                    return;
                }
                _isBuilding = true;
                _buildingRoutine = StartCoroutine(StartBuild(buildZone));
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            if(other.CompareTag("BuildZone"))
            {
                _isBuilding = false;
                if(_buildingRoutine != null)
                {
                    StopCoroutine(_buildingRoutine);
                    _buildingRoutine = null;
                }
            }
        }

        private IEnumerator StartBuild(BuildZone buildZone)
        {
            BuildableItem buildableItem = buildZone.GetBuildableItem();

            while(buildableItem.GetRequiredMoneyLeft() > 0 && buildableItem != null && PlayerData.Instance.CurrencyAmount >= 1 && buildableItem.GetCanReceiveMoney())
            {
                if(!_movementController.GetIsMoving())
                {
                    PlayPayAnimation(buildZone);
                }
                yield return new WaitForSeconds(.01f);
            }
            _isBuilding = false;
        }

        private void PlayPayAnimation(BuildZone buildZone)
        {
            BuildableItem buildableItem = buildZone.GetBuildableItem();
            PlayerData.Instance.PayCurrency(1);
            buildableItem.PayForItem();

            GameObject moneyInstance = ObjectPool.instance.SpawnFromPool("Money", transform.position, Quaternion.identity);
            JumpAnimator.instance.MoveTargetToPosition(moneyInstance.transform, buildZone.transform.position, .2f, 2.5f, OnJumpComplete);

            void OnJumpComplete()
            {
                moneyInstance.SetActive(false);
            }
        }

        protected override void OnEnteredCarTrigger(CarController triggeredCar)
        {
            StopEnteringCar();
            
            StartEnteringCar(triggeredCar);
        }
        
        protected override void OnExitCarTrigger()
        {
            StopEnteringCar();
        }

        protected override void OnEnteredCar(CarController enteredCar)
        {
            SetEnteringCar(null);
            base.OnEnteredCar(enteredCar);

            EnteredCar?.Invoke(enteredCar);
        }

        protected override void OnLeftCar()
        {
            SetEnteringCar(null);

            LeftCar?.Invoke();

            base.OnLeftCar();
        }

        private void StartEnteringCar(CarController targetCar)
        {
            SetEnteringCar(targetCar);
            _enteringCar.SetCarBeingEntered(true);

            _enterCarRoutine = StartCoroutine(EnterCarRoutine());

            StartedEnteringCar?.Invoke(targetCar);
        }

        private void StopEnteringCar()
        {
            SetEnteringCar(null);
            if(_enterCarRoutine != null)
            {
                StopCoroutine(_enterCarRoutine);
                _enterCarRoutine = null;
            }

            ResetEnterCarPercentage();

            StoppedEnteringCar?.Invoke();
        }

        private IEnumerator EnterCarRoutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(.05f);

                IncrementEnterCarPercentage();
                if(GetEnterCarPercentage() >= 1)
                {
                    if(TryEnterCar())
                    {
                        _enterCarRoutine = null;
                        OnEnteredCar(_enteringCar);
                    }
                    break;
                }
            }
        }

        public void IncrementEnterCarPercentage()
        {
            SetEnterCarPercentage(_enterCarPercentage + .05f);
        }
        
        private bool TryEnterCar()
        {
            if(_enteringCar.GetCanEnterCar())
            {
                return true;
            }
            return false;
        }

        private void ResetEnterCarPercentage()
        {
            SetEnterCarPercentage(0);
        }

        private void SetEnteringCar(CarController enteringCar)
        {
            _enteringCar = enteringCar;
        }

        private void SetEnterCarPercentage(float newPercentage)
        {
            _enterCarPercentage = newPercentage;

            EnterCarPercentageChanged?.Invoke();
        }

        public float GetEnterCarPercentage()
        {
            return _enterCarPercentage;
        }
    }
}