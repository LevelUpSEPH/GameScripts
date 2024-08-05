using System;
using System.Collections.Generic;

namespace Chameleon.Game.ArcadeIdle
{
    public class SpecialCustomerStockPlace : PoopStockPlace
    {
        public event Action<PoopType> PoopReceived;
        
        public PoopSlotWithType GetPoopSlotAtIndex(int index)
        {
            return _poopSlotsManager.GetSlotAtIndex(index).GetComponent<PoopSlotWithType>();
        }

        public void ResetSlots()
        {
            _poopSlotsManager.ResetSlots();
        }

        public int GetSlotCount()
        {
            return _poopSlotsManager.GetSlotCount();
        }

        protected override void OnPoopPlaced(PoopBase poop)
        {
            PoopType poopType = poop.GetPoopType();
            PoopReceived?.Invoke(poopType);
        }

        public List<PoopBase> GetEveryPoop()
        {
            return _poopSlotsManager.GetPoops();
        }
    }
}