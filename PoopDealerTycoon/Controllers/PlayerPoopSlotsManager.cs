using System;

namespace Chameleon.Game.ArcadeIdle
{
    public class PlayerPoopSlotsManager : UnitPoopSlotsManager
    {
        public event Action AllSlotsFilled;
        public event Action SomeSlotsEmptied;

        protected override void OnSlotCleared(PoopSlot clearedSlot)
        {
            base.OnSlotCleared(clearedSlot);
            HandleSlotsFilledStatus();
        }

        protected override void OnSlotFilled(PoopSlot poopSlot)
        {
            base.OnSlotFilled(poopSlot);
            HandleSlotsFilledStatus();
        }

        private void HandleSlotsFilledStatus()
        {
            if(GetIsFull())
                AllSlotsFilled?.Invoke();
            else
                SomeSlotsEmptied?.Invoke();
        }

        protected override void OnActiveSlotCountChanged()
        {
            SomeSlotsEmptied?.Invoke();
        }
    }
}