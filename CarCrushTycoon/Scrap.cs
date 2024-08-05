using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class Scrap : MonoBehaviour
    {
        [SerializeField] private int _scrapSellPrice;
        private bool _canBeCollected = true;

        private void OnEnable()
        {
            ResetScrap();
        }

        private void ResetScrap()
        {
            _canBeCollected = true;
        }

        public int GetSellPrice()
        {
            return _scrapSellPrice;
        }

        public bool GetCanBeCollected()
        {
            return _canBeCollected;
        }

        public void SetCanBeCollected(bool canBeCollected)
        {
            _canBeCollected = canBeCollected;
        }
    }
}