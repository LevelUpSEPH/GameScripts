using System;
using Chameleon.Game.ArcadeIdle.Helpers;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopShowPlace : PoopStockWithAcceptedType
    {
        public static event Action ShowplaceActivated;
        public event Action PoopCountChanged;

        protected override void Start()
        {
            base.Start();
            PoopTypeActivated();
            ShowplaceActivated?.Invoke();
        }

        public int GetCurrentCount()
        {
            return _poopSlotsManager.GetFullSlotCount();
        }

        public int GetMaxCount()
        {
            return _poopSlotsManager.GetSlotCount();
        }

        protected override void OnPoopPlaced(PoopBase placedPoop)
        {
            placedPoop.SetIsOnShow(true);
        }

        protected override void OnPoopCountChanged()
        {
            PoopCountChanged?.Invoke();
        }

        private void PoopTypeActivated()
        {
            PoopTypeActivityController.instance.SetIsPoopTypeActive(_acceptedPoopType, true);
        }

        public override bool GetIsShowPlace()
        {
            return true;
        }

    }
}