using System;
using Chameleon.Game.ArcadeIdle.Movement;
using Chameleon.Game.ArcadeIdle.Checkout;
using Chameleon.Game.ArcadeIdle.Units;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Commands
{
    public class GotoCheckoutCommand : AICommand
    {
        private CustomerUnit targetCustomerUnit;
        public override void PlayCommand(BaseAIMovementController baseAI, Action onCompleteAction = null)
        {
            base.PlayCommand(baseAI, onCompleteAction);
            CustomerUnit customerUnit = baseAI.GetComponent<CustomerUnit>();
            targetCustomerUnit = customerUnit;
            CheckoutLineController.instance.AddUnitToCashoutList(customerUnit);
            targetCustomerUnit.CashedOut += CompleteCommand;
        }

        public override void CompleteCommand()
        {
            targetCustomerUnit.CashedOut -= CompleteCommand;
            base.CompleteCommand();
        }

    }
}
