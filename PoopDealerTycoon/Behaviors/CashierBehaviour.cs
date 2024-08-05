using UnityEngine;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle.Checkout
{
    public class CashierBehaviour : MonoBehaviour
    {
        [SerializeField] private CheckoutBehaviour _checkout;
        private void OnEnable()
        {
            PlayerData.Instance.IsPermanentCheckoutActive = true;
            _checkout.SetPermanentCheckoutActive(true);
        }
    }
}