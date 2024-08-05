using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopStockPlace : StockPlaceBase // this one will be for normal placements
    {
        [SerializeField] protected PoopSlotsManager _poopSlotsManager;

        public bool TryGetSlotWithPoop(out PoopSlot poopSlot)
        {
            for(int i = _poopSlotsManager.GetSlotCount() - 1; i >= 0; i--)
            {
                PoopSlot iteratingPoopSlot = _poopSlotsManager.GetSlotAtIndex(i);
                if(iteratingPoopSlot.IsFull)
                {
                    poopSlot = iteratingPoopSlot;
                    return true;
                }
            }
            poopSlot = null;
            return false;
        }

        public override bool GetCanReceivePoop(PoopBase poop)
        {
            return !_poopSlotsManager.GetIsFull();
        }

        public bool GetIsEmpty()
        {
            return !_poopSlotsManager.GetHasPoop();
        }

        protected virtual void OnPoopPlaced(PoopBase poop)
        {

        }

        protected virtual void OnPoopCountChanged()
        {

        }

        public override bool TryDropPoop(PoopBase poopBase)
        {
            if(GetCanReceivePoop(poopBase))
            {
                if(TryPlacePoop(poopBase))
                    return true;
                else
                    return false;
            }
            return false;
        }

        private bool TryPlacePoop(PoopBase poop)
        {
            if(_poopSlotsManager.TryGetEmptyPoopSlot(out PoopSlot emptySlot))
            {
                emptySlot.TryPlaceInsideSlot(poop, () => 
                {
                    OnPoopPlaced(poop);
                    OnPoopCountChanged();
                });
                return true;
            }
            else
                return false;
        }

        public override PoopBase GetExamplePoop()
        {
            if(!_poopSlotsManager.GetHasPoop())
                return null;
            else
            {
                return _poopSlotsManager.GetPoops()[_poopSlotsManager.GetFullSlotCount() - 1];
            }
        }

        public override bool GetIsShowPlace()
        {
            return false;
        }

        public override PoopType GetAcceptedPoopType()
        {
            return PoopType.None;
        }

        public override void CollectPoop(PoopBase poopBase)
        {
            _poopSlotsManager.TryGetSlotOfPoop(out PoopSlot slotOfPoop, poopBase);
            slotOfPoop.ClearSlot();
            OnPoopCountChanged();
        }
    }
}