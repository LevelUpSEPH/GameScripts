using System;
using System.Collections;
using Chameleon.Game.ArcadeIdle.Units;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class PoopShowPlacePosition : PositionBase
    {
        public event Action<PoopShowPlacePosition, bool> PlaceAvailabilityChanged;

        protected override IEnumerator SetUnitIfStoppedRoutine(CustomerUnit customerUnit)
        {
            while(true)
            {
                yield return null;
                if(!customerUnit.GetIsMoving())
                {
                    SetCustomerInPosition(customerUnit);
                    InvokePlaceAvailabilityChanged(false);
                    break;
                }
            }
        }

        protected override void OnCustomerLeftPosition()
        {
            SetCustomerInPosition(null);
            InvokePlaceAvailabilityChanged(true);
        }

        private void InvokePlaceAvailabilityChanged(bool isPlaceAvailable)
        {
            PlaceAvailabilityChanged?.Invoke(this, isPlaceAvailable);
        }

        protected override bool IsTriggeringWithUnitAvailable(CustomerUnit triggeredCustomerUnit)
        {
            return _customerUnitInPosition == null;
        }
    }
}