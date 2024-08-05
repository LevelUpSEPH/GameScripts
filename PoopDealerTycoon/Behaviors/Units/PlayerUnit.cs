using System.Collections;
using UnityEngine;
using System;
using Game.Scripts.Models;
using Chameleon.Game.ArcadeIdle.Helpers;
using Chameleon.Game.ArcadeIdle.Zones;
using Chameleon.Game.ArcadeIdle.Abstract;
using Lofelt.NiceVibrations;

namespace Chameleon.Game.ArcadeIdle.Units
{
    public class PlayerUnit : WorkerUnit
    {
        public static event Action PlayerPickedPoop;
        public static event Action PlayerCouldntCollectFromZone;

        private bool _isBuilding = false;
        private bool _isTrashing = false;
        private Coroutine _trashingRoutine;
        private Coroutine _buildingRoutine;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if(other.CompareTag("BuildZone"))
            {
                BuildZone buildZone = other.GetComponent<BuildZone>();
                if(_isBuilding)
                    return;
                _isBuilding = true;
                _buildingRoutine = StartCoroutine(StartBuild(buildZone));
            }
            else if(other.CompareTag("TrashPoopZone"))
            {
                if(_isTrashing)
                    return;
                _isTrashing = true;
                PoopTrasher poopTrasher = other.GetComponent<PoopTrashZone>().GetPoopTrasher();
                _trashingRoutine = StartCoroutine(StartTrashPoop(poopTrasher));
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
            else if(other.CompareTag("TrashPoopZone"))
            {
                if(_trashingRoutine != null)
                {
                    StopCoroutine(_trashingRoutine);
                    _trashingRoutine = null;
                }
                _isTrashing = false;
            }
        }

        protected override void OnPickedUp(PoopBase pickedPoop)
        {
            base.OnPickedUp(pickedPoop);
            PlayerPickedPoop?.Invoke();
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }

        protected override bool CanPickupPoop(PoopBase poop)
        {
            return !_poopSlotsManager.GetIsFull() && _poopSlotsManager.GetHasEmptyActiveSlot() && !poop.GetIsOnShow();
        }

        protected override void OnPoopTypeInactive()
        {
            PlayerCouldntCollectFromZone?.Invoke();
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

        public override void DropOffInside(IDropPlace dropPlace)
        {
            if(GetDropPlaceHasType(dropPlace))
            {
                PlaceInsideSlotWithAcceptedType(dropPlace);
            }
            else
            {
                PlaceInsideTypelessSlot(dropPlace);
            }
        }

        private void PlaceInsideSlotWithAcceptedType(IDropPlace dropPlace)
        {
            if(_poopSlotsManager.TryGetFullSlotWithType(out PoopSlot poopSlot, dropPlace.GetAcceptedPoopType()))
            {
                PlacePoop(dropPlace, poopSlot);
            }
        }

        private void PlaceInsideTypelessSlot(IDropPlace dropPlace)
        {
            if(_poopSlotsManager.TryGetFullSlot(out PoopSlot poopSlot))
            {
                PlacePoop(dropPlace, poopSlot);
            }
        }

        private void PlacePoop(IDropPlace dropPlace, PoopSlot poopSlot)
        {
            PoopBase poop = poopSlot.GetPoopInSlot();
            if(dropPlace.GetCanReceivePoop(poop))
            {
                poopSlot.ClearSlot();
                if(!dropPlace.TryDropPoop(poop))
                {
                    poop.gameObject.SetActive(false);
                }
                else
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                HandleCarryingAnimation();
            }
        }

        private IEnumerator StartTrashPoop(PoopTrasher poopTrasher)
        {
            yield return null;
            while(_poopSlotsManager.GetHasPoop())
            {
                yield return new WaitForSeconds(.2f);
                if(_poopSlotsManager.TryGetFullSlot(out PoopSlot poopSlot))
                {
                    poopTrasher.TrashPoop(poopSlot.GetPoopInSlot());
                    poopSlot.ClearSlot();
                    HandleCarryingAnimation();
                }
            }
        }

        protected override void SetTargetUpgradeSkill()
        {
            _targetUpgradeSkill = Helpers.UpgradeSkillDict.upgradableSkillsDict[UpgradeSkillName.PlayerStack];
        }

        private bool GetDropPlaceHasType(IDropPlace dropPlace)
        {
            return dropPlace.GetAcceptedPoopType() != PoopType.None;
        }

    }
}
