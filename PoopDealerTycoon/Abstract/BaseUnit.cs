using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.Animation;
using Chameleon.Game.ArcadeIdle.Zones;
using System;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public class BaseUnit : MonoBehaviour, IPicker
    {
        public event Action<PoopBase> PickedPoop;
        [SerializeField] protected UnitPoopSlotsManager _poopSlotsManager;
        private SimpleAnimationController _animationController;
        protected BaseMovementController _movementController;
        private bool _isCollectingFromZone = false;
        protected bool _canPickPoop = false;

        private Coroutine _collectionRoutine;

        protected virtual void Start()
        {
            _movementController = GetComponent<BaseMovementController>();
            if(TryGetComponent<SimpleAnimationController>(out SimpleAnimationController animationController))
                _animationController = animationController;
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("CollectPoopZone"))
            {
                if(_isCollectingFromZone)
                    return;
                PoopCollectionZone poopCollectionZone = other.GetComponent<PoopCollectionZone>();
                _collectionRoutine = StartCoroutine(StartCollectionFromZone(poopCollectionZone));
                _isCollectingFromZone = true;
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("CollectPoopZone"))
            {
                if(_collectionRoutine != null)
                {
                    StopCoroutine(_collectionRoutine);
                    _collectionRoutine = null;
                }
                _isCollectingFromZone = false;
            }
        }

        private IEnumerator StartCollectionFromZone(PoopCollectionZone poopCollectionZone)
        {
            StockPlaceBase collectionPlace = poopCollectionZone.GetCollectionPlace();
            if(CanCollectFromZone(poopCollectionZone))
            {
                while(_poopSlotsManager.GetHasEmptyActiveSlot())
                {
                    PoopBase poop = collectionPlace.GetExamplePoop();
                    if(_movementController.GetIsMoving())
                    {
                        // Dont do anything
                    }
                    else if(poop != null)
                    {
                        if(!IsPoopTypeActive(poop.GetPoopType()))
                        {
                            OnPoopTypeInactive();
                            yield return new WaitForSeconds(3f);
                        }
                        else
                        {
                            if(!TryPickup(poop))
                            {
                                FailToPickup();
                                yield return null;
                            }
                            else
                            {
                                collectionPlace.CollectPoop(poop);
                                OnPickedUp(poop);
                                yield return new WaitForSeconds(.2f);
                            }
                        }
                    }
                    else
                    {
                        FailToPickup();
                    }
                    yield return null;
                }
                FailToPickup();
            }
        }

        protected virtual void OnPickedUp(PoopBase pickedPoop)
        {
            PickedPoop?.Invoke(pickedPoop);
        }

        protected virtual bool IsPoopTypeActive(PoopType poopType)
        {
            return Helpers.PoopTypeActivityController.instance.GetIsPoopTypeActive(poopType);
        }

        protected virtual void OnPoopTypeInactive()
        {

        }

        protected virtual void FailToPickup()
        {

        }

        protected virtual bool CanCollectFromZone(PoopCollectionZone poopCollectionZone)
        {
            return true;
        }

        public void SetCanPickPoop(bool canPickPoop)
        {
            _canPickPoop = canPickPoop;
        }

        private bool TryPickup(PoopBase poop)
        {
            if(CanPickupPoop(poop))
            {
                PickUp(poop);
                return true;
            }
            return false;
        }

        protected virtual bool CanPickupPoop(PoopBase poop)
        {
            return _poopSlotsManager.GetHasEmptyActiveSlot() && _canPickPoop &&
            !_poopSlotsManager.GetIsFull();
        }

        public virtual void PickUp(PoopBase poop)
        {
            if(_poopSlotsManager.TryGetEmptyPoopSlot(out PoopSlot emptySlot))
            {
                PlaceInsideEmptySlot(poop, emptySlot);
            }
        }

        protected void PlaceInsideEmptySlot(PoopBase poop, PoopSlot emptySlot)
        {
            emptySlot.TryPlaceInsideSlot(poop);
            HandleCarryingAnimation();
        }
        
        protected void HandleCarryingAnimation()
        {
            if(_animationController != null)
                _animationController.SetCarryingAnimationPlaying(_poopSlotsManager.GetHasPoop());
        }

        public List<PoopBase> GetEveryPoop()
        {
            return _poopSlotsManager.GetPoops();
        }

        public int GetActiveSlotCount()
        {
            return _poopSlotsManager.GetActiveSlotCount();
        }

    }
}
