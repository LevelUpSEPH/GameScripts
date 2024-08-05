using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class StarterMoneyGiver : MonoBehaviour
    {
        [SerializeField] private int _starterMoneyAmount = 100;
        [SerializeField] private MoneyStackingArea _moneyStackingArea;
        private void Start()
        {
            if(!PlayerData.Instance.IsStartCurrencyAdded)
            {
                SpawnStartingMoney();
            }
        }

        private void OnDisable()
        {
            PlayerData.Stats[StatKeys.CurrencyAmount].Changed -= OnPlayerTookStarterMoney;
        }

        private void SpawnStartingMoney()
        {
            _moneyStackingArea.AddMoney(_starterMoneyAmount);
            PlayerData.Stats[StatKeys.CurrencyAmount].Changed += OnPlayerTookStarterMoney;
        }

        private void OnPlayerTookStarterMoney()
        {
            PlayerData.Instance.IsStartCurrencyAdded = true;
            PlayerData.Stats[StatKeys.CurrencyAmount].Changed -= OnPlayerTookStarterMoney;
        }
    }
}