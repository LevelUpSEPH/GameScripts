using System.Collections;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;
using Chameleon.Game.ArcadeIdle.Zones;
using Chameleon.Game.ArcadeIdle.Upgrade;
using System;

namespace Chameleon.Game.ArcadeIdle.Units
{
    public class WorkerUnit : BaseUnit, IDropper
    {
        public event Action DroppedPoop;
        public event Action DisabledPoop;
        public event Action FailedToPickPoop;
        
        private bool _isDroppingIntoZone = false;
        protected bool _canDropPoop = false;
        protected UpgradeSkill _targetUpgradeSkill;
        private Coroutine _dropRoutine;

        protected override void Start()
        {
            base.Start();
            SetTargetUpgradeSkill();
            UpdateActiveSlotCount();
            RegisterEvents();
        }

        protected virtual void OnDisable()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            _targetUpgradeSkill.UpgradeEvent += UpdateActiveSlotCount;
        }

        private void UnregisterEvents()
        {
            _targetUpgradeSkill.UpgradeEvent -= UpdateActiveSlotCount;
        }

        protected virtual void SetTargetUpgradeSkill()
        {

        }
        
        protected void UpdateActiveSlotCount()
        {
            _poopSlotsManager.ActivateSlots((int)_targetUpgradeSkill.GetCurrentLevelCoef());
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if(other.CompareTag("DropPoopZone"))
            {
                if(_isDroppingIntoZone)
                    return;
                IDropPlace poopShowplace = other.GetComponent<PoopDropZone>().GetPoopDropPlace();
                _dropRoutine = StartCoroutine(StartDropToZone(poopShowplace));
                _isDroppingIntoZone = true;
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            if(other.CompareTag("DropPoopZone"))
            {
                if(_dropRoutine != null)
                {
                    StopCoroutine(_dropRoutine);
                    _dropRoutine = null;
                }
                _isDroppingIntoZone = false;
            }
        }

        protected override bool CanPickupPoop(PoopBase poop)
        {
            return base.CanPickupPoop(poop) &&
            !poop.GetIsOnShow();
        }

        protected override bool CanCollectFromZone(PoopCollectionZone poopCollectionZone)
        {
            ICollectionPlace collectionPlace = poopCollectionZone.GetCollectionPlace();

            return base.CanCollectFromZone(poopCollectionZone) && !collectionPlace.GetIsShowPlace();
        }

        private IEnumerator StartDropToZone(IDropPlace dropPlace)
        {
            yield return null;
            while(_poopSlotsManager.GetHasPoop())
            {
                DropOffInside(dropPlace);
                yield return new WaitForSeconds(.2f);
            }
        }

        public virtual void DropOffInside(IDropPlace dropPlace)
        {
            if(!CanDropPoop())
            {
                return;
            }
            if(_poopSlotsManager.TryGetFullSlot(out PoopSlot poopSlot))
            {
                PoopBase poop = poopSlot.GetPoopInSlot();
                if(dropPlace.GetCanReceivePoop(poop))
                {
                    poopSlot.ClearSlot();
                    if(dropPlace.TryDropPoop(poop))
                    {
                        OnPlacedPoop();
                    }
                    else
                    {
                        DestroyCarryingPoop(poopSlot);
                    }
                    HandleCarryingAnimation();
                }
                else
                {
                    DestroyCarryingPoop(poopSlot);
                }
            }
        }

        protected virtual void OnPlacedPoop()
        {
            InvokeDroppedPoop();
        }

        protected bool CanDropPoop()
        {
            return _canDropPoop;
        }

        public void SetCanDropPoop(bool canDropPoop)
        {
            _canDropPoop = canDropPoop;
        }

        protected void InvokeDroppedPoop()
        {
            DroppedPoop?.Invoke();
        }

        protected override void FailToPickup()
        {
            FailedToPickPoop?.Invoke();
        }

        private void DestroyCarryingPoop(PoopSlot poopSlot)
        {
            PoopBase poop = poopSlot.GetPoopInSlot();
            poop.DisablePoop();
            poopSlot.ClearSlot();
            DisabledPoop?.Invoke();
            HandleCarryingAnimation();
        }

    }
}