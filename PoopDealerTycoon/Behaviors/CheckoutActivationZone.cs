using System.Collections;
using System.Collections.Generic;
using Chameleon.Game.ArcadeIdle.Checkout;
using UnityEngine;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle.Zones
{
    public class CheckoutActivationZone : TriggerZone
    {
        [SerializeField] private CheckoutBehaviour _targetCashout;
        protected override void OnPlayerEnteredTrigger()
        {
            if(PlayerData.Instance.IsPermanentCheckoutActive)
                return;
            base.OnPlayerEnteredTrigger();
            _targetCashout.SetCheckoutActive(true);
        }

        protected override void OnPlayerExitTrigger()
        {
            if(PlayerData.Instance.IsPermanentCheckoutActive)
                return;
            base.OnPlayerExitTrigger();
            _targetCashout.SetCheckoutActive(false);
        }
        
    }
}