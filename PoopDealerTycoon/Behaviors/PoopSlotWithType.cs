using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopSlotWithType : PoopSlot
    {
        [SerializeField] private PoopType _acceptedPoopType;

        protected override bool CanPlaceInsideSlot(PoopBase poop)
        {
            return base.CanPlaceInsideSlot(poop) && _acceptedPoopType == poop.GetPoopType();
        }

        public void SetAcceptedPoopType(PoopType poopType)
        {
            _acceptedPoopType = poopType;
        }

        public PoopType GetAcceptedPoopType()
        {
            return _acceptedPoopType;
        }
    }
}