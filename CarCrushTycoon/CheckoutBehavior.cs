using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Helpers;
using PlayerData = Game.Scripts.Models.PlayerData;

namespace Chameleon.Game.ArcadeIdle
{
    public class CheckoutBehavior : MonoBehaviour
    {
        [SerializeField] private Transform _checkoutFinalPosition;
        [SerializeField] private MoneyStackingArea _targetMoneyStackingArea;

        public void TurnScrapIn(Scrap turningInScrap)
        {
            JumpAnimator.instance.MoveTargetToPosition(turningInScrap.transform, _checkoutFinalPosition.position, .5f, () => OnScrapReachedPoint(turningInScrap));
        }

        private void OnScrapReachedPoint(Scrap scrap)
        {
            TurnScrapIntoMoney(scrap); // temporary, the animations will change
            scrap.gameObject.SetActive(false);
        }

        private void TurnScrapIntoMoney(Scrap scrapToTurn)
        {
            float sellPriceMultiplier = UpgradeController.instance.income[PlayerData.Instance.IncomeLevel - 1].coef;
            float multipliedSellPrice = scrapToTurn.GetSellPrice() * sellPriceMultiplier;

            _targetMoneyStackingArea.AddMoney((int)Mathf.Ceil(multipliedSellPrice));
        }
    }
}