using System;
using System.Collections;
using Chameleon.Game.ArcadeIdle.Abstract;
using Chameleon.Game.ArcadeIdle.Units;

namespace Chameleon.Game.ArcadeIdle.Checkout
{
    public class CheckoutLinePosition : PositionBase
    {
        public event Action LinePositionAvailabilityChanged;
        private bool _isFirstSpot;
        
        protected override IEnumerator SetUnitIfStoppedRoutine(CustomerUnit customerUnit)
        {
            while(true)
            {
                yield return null;
                if(!customerUnit.GetIsMoving())
                {
                    if(_isFirstSpot)
                    {
                        _customerUnitInPosition.InvokeCameToFirstPlace();
                    }
                    else
                    {
                        RotateUnitToFaceForward(customerUnit);
                    }

                    customerUnit.SetIsInLine(true);
                    LinePositionAvailabilityChanged?.Invoke();

                    break;
                }
            }
        }

        private void RotateUnitToFaceForward(CustomerUnit customerUnit)
        {
            
        }

        public CustomerUnit GetCustomerUnitInPosition()
        {
            return _customerUnitInPosition;
        }

        protected override bool IsTriggeringWithUnitAvailable(CustomerUnit triggeredCustomerUnit)
        {
            return _customerUnitInPosition == triggeredCustomerUnit;
        }

        protected override void OnCustomerLeftPosition()
        {
            _customerUnitInPosition.SetIsInLine(false);
            SetCustomerInPosition(null);

            LinePositionAvailabilityChanged?.Invoke();
        }

        public void SetIsFirstSpot(bool isFirstSpot)
        {
            _isFirstSpot = isFirstSpot;
        }

    }
}