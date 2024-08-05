using UnityEngine;
using System;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopSlot : MonoBehaviour
    {
        public event Action<PoopSlot> SlotFilled;
        public event Action<PoopSlot> SlotCleared;

        [SerializeField] private Transform _poopPlacementTargetTransform;
        [SerializeField] private PoopBase _poopInSlot = null;
        [SerializeField] private bool _isSlotActive = false;
        public bool IsFull => _poopInSlot != null;
        private bool _isSlotAnimating = false;

        public bool TryPlaceInsideSlot(PoopBase poop)
        {
            if(CanPlaceInsideSlot(poop))
            {
                _poopInSlot = poop;
                poop.MoveToPosition(_poopPlacementTargetTransform.position, SetNewParent);
                void SetNewParent()
                {
                    SetPoopTransform(poop);
                }
                return true;
            }
            return false;
        }

        public bool TryPlaceInsideSlot(PoopBase poop, Action onCompleteAction = null)
        {
            if(CanPlaceInsideSlot(poop))
            {
                _isSlotAnimating = true;
                poop.MoveToPosition(_poopPlacementTargetTransform.position, OnAnimationFinished);

                void OnAnimationFinished()
                {
                    SetPoopTransform(poop);
                    _poopInSlot = poop;
                    _isSlotAnimating = false;
                    onCompleteAction?.Invoke();
                }
                return true;
            }
            return false;
        }

        private void SetPoopTransform(PoopBase poop)
        {
            poop.transform.parent = _poopPlacementTargetTransform;
            poop.transform.position = _poopPlacementTargetTransform.position;
            poop.transform.rotation = _poopPlacementTargetTransform.rotation;

            SlotFilled?.Invoke(this);
        }

        protected virtual bool CanPlaceInsideSlot(PoopBase poop)
        {
            return !IsFull && GetIsSlotActive() && !_isSlotAnimating;
        }

        public bool GetIsSlotAvailable()
        {
            return !_isSlotAnimating;
        }

        public bool TryGetPoopInSlot(out PoopBase poop)
        {
            poop = _poopInSlot;
            if(IsFull)
            {
                return true;
            }
            else
                return false;
        }

        public void SetSlotActive(bool isSlotActive)
        {
            _isSlotActive = isSlotActive;
        }

        public bool GetIsSlotActive()
        {
            return _isSlotActive;
        }

        public PoopBase GetPoopInSlot()
        {
            return _poopInSlot;
        }

        public void ClearSlot()
        {
            if(_poopInSlot == null)
                return;
            if(_poopInSlot.transform.parent == _poopPlacementTargetTransform)
            {
                Helpers.ParentRemover.instance.RemoveParentFrom(_poopInSlot.transform);
            }
            _poopInSlot = null;
            SlotCleared?.Invoke(this);
        }
    }
}
