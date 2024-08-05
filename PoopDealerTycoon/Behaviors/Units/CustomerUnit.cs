using Chameleon.Game.ArcadeIdle.Abstract;
using System;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Helpers;
using Chameleon.Game.ArcadeIdle.Zones;

namespace Chameleon.Game.ArcadeIdle.Units
{
    public class CustomerUnit : BaseUnit
    {
        public static event Action<CustomerUnit> CameToFirstPlace;
        public static event Action CustomerDisabled;
        public static event Action SomeCustomerPickedPoop;
        public event Action CustomerInitialized;
        public event Action NeedsChanged;
        public event Action CashedOut;
        public event Action CustomerLeft;
        [SerializeField] private Transform _boxCarryingTransform;
        [SerializeField] private List<GameObject> _accessories = new List<GameObject>();

        private CustomerNeeds _customerNeeds = new CustomerNeeds();
        private GameObject _carryingBox;
        private bool _cameToFirstPlace = false;
        private bool _isInLine = false;
        private bool _isLeaving = false;
        
        protected override bool CanPickupPoop(PoopBase poop)
        {            
            return base.CanPickupPoop(poop) &&
            _poopSlotsManager.GetHasEmptySlotWithType(poop.GetPoopType());
        }

        private void OnEnable()
        {
            ActivateRandomAccessory();
            ResetUnit();
        }

        private void OnDisable()
        {
            DisableCarryingBox();
            ResetAcceptedTypes();
            _customerNeeds.ResetNeeds();
            DisableAllAccessories();
            _poopSlotsManager.DisableSlots();
        }

        private void ResetUnit()
        {
            _poopSlotsManager.DisableSlots();
            HandleCarryingAnimation();
            _cameToFirstPlace = false;
            _isInLine = false;
            _isLeaving = false;
            gameObject.name = "Customer #" + UnityEngine.Random.Range(1000, 10000);
        }

        private void ActivateRandomAccessory()
        {
            int randomAccessoryIndex = UnityEngine.Random.Range(0, _accessories.Count);
            _accessories[randomAccessoryIndex].SetActive(true);
        }

        private void DisableAllAccessories()
        {
            foreach(GameObject accessory in _accessories)
            {
                accessory.SetActive(false);
            }
        }

        protected override void OnPickedUp(PoopBase pickedPoop)
        {
            base.OnPickedUp(pickedPoop);
            SomeCustomerPickedPoop?.Invoke();
        }

        public void Leave()
        {
            if(_isLeaving)
                return;
            _isLeaving = true;
            CustomerDisabled?.Invoke();
            CustomerLeft?.Invoke();
        }

        private void ResetAcceptedTypes()
        {
            int slotCount = _poopSlotsManager.GetSlotCount();
            for(int i = 0; i < slotCount; i++)
            {
                PoopSlotWithType poopSlot = _poopSlotsManager.GetSlotAtIndex(i).GetComponent<PoopSlotWithType>();
                poopSlot.SetAcceptedPoopType(PoopType.None);
            }
        }

        public void SetCarryingBox(GameObject carryingBox)
        {
            carryingBox.transform.parent = _boxCarryingTransform;
            carryingBox.transform.localRotation = _boxCarryingTransform.localRotation;
            _carryingBox = carryingBox;
        }

        private void DisableCarryingBox()
        {
            if(_carryingBox == null)
                return;
            _carryingBox.GetComponent<BoxAnimator>().RemoveParent();
            _carryingBox = null;
        }

        protected override bool CanCollectFromZone(PoopCollectionZone poopCollectionZone)
        {
            ICollectionPlace collectionPlace = poopCollectionZone.GetCollectionPlace();
            if(collectionPlace.GetIsShowPlace())
            {
                return base.CanCollectFromZone(poopCollectionZone);
            }
            else
            {
                return false;
            }
        }

        public override void PickUp(PoopBase poop)
        {
            if(_poopSlotsManager.TryGetEmptySlotWithType(out PoopSlot emptySlot, poop.GetPoopType()))
            {
                PlaceInsideEmptySlot(poop, emptySlot);
                _customerNeeds.RemoveFromNeeds(poop.GetPoopType());
                NeedsChanged?.Invoke();
            }
        }

        public void SetWants(PoopType[] poopTypes)
        {
            for(int i = 0; i < poopTypes.Length; i++)
            {
                PoopSlotWithType poopSlot = _poopSlotsManager.GetSlotAtIndex(i).GetComponent<PoopSlotWithType>();
                if(poopTypes[i] == PoopType.None)
                {
                    poopSlot.SetSlotActive(false);        
                    continue;
                }
                poopSlot.SetAcceptedPoopType(poopTypes[i]);
                _customerNeeds.AddToNeeds(poopTypes[i]);
                poopSlot.SetSlotActive(true);
            }
            CustomerInitialized?.Invoke();
        }

        public void InvokeCameToFirstPlace()
        {
            if(!_cameToFirstPlace)
            {
                Transform tableTransform = ScenePointManager.instance.GetCheckoutTableTransform();
                RotateAnimator.instance.RotateTowards(transform, tableTransform, .5f, () => CameToFirstPlace?.Invoke(this));
                _cameToFirstPlace = true;
            }
        }

        public void InvokeCashedOut()
        {
            CashedOut?.Invoke();
        }

        public PoopType GetNextWant()
        {
            return _customerNeeds.GetNextNeed();
        }

        public bool GetCameToFirstPlace()
        {
            return _cameToFirstPlace;
        }

        public void RemoveNextWant()
        {
            _customerNeeds.RemoveFromNeeds(_customerNeeds.GetNextNeed());
            NeedsChanged?.Invoke();
        }

        public void ClearWants()
        {
            _customerNeeds.ResetNeeds();
            NeedsChanged?.Invoke();
        }

        public bool GetIsMoving()
        {
            return _movementController.GetIsMoving();
        }

        public void SetIsInLine(bool isInLine)
        {
            _isInLine = isInLine;
        }

        public bool GetIsInLine()
        {
            return _isInLine;
        }
    }
}
