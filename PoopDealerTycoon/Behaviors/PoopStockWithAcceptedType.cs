using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopStockWithAcceptedType : PoopStockPlace
    {
        [SerializeField] protected PoopType _acceptedPoopType;

        protected virtual void Start()
        {
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            for(int i = 0; i < _poopSlotsManager.GetSlotCount(); i++)
            {
                PoopSlotWithType poopSlotWithType = _poopSlotsManager.GetSlotAtIndex(i).GetComponent<PoopSlotWithType>();
                poopSlotWithType.SetAcceptedPoopType(_acceptedPoopType);
                poopSlotWithType.SetSlotActive(true);
            }
        }

        public override PoopType GetAcceptedPoopType()
        {
            return _acceptedPoopType;
        }

        public override bool GetCanReceivePoop(PoopBase poop)
        {
            return base.GetCanReceivePoop(poop) && 
            _acceptedPoopType == poop.GetPoopType();
        }
    }
}
