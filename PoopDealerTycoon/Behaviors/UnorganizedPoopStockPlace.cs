using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle
{
    public class UnorganizedPoopStockPlace : StockPlaceBase
    {
        [SerializeField] private int _maxAmountOfPoop = 10;
        [SerializeField] protected List<PoopBase> _poopsInStock = new List<PoopBase>();
        [SerializeField] private int _waitingPoopAmount = 0;

        public override bool TryDropPoop(PoopBase poopBase)
        {
            if(!GetCanReceivePoop(poopBase))
                return false;

            AddToPoopsInStock(poopBase);
            return true;
        }

        public void AddToPoopsInStock(PoopBase poop)
        {
            _poopsInStock.Add(poop);
        }

        public void RemoveFromPoopsInStock(PoopBase poop)
        {
            _poopsInStock.Remove(poop);
        }

        public void DecrementWaitingPoopAmount()
        {
            _waitingPoopAmount--;
        }

        public void IncrementWaitingPoopAmount()
        {
            _waitingPoopAmount++;
        }

        public override PoopBase GetExamplePoop()
        {
            if(_poopsInStock.Count <= 0)
            {
                return null;
            }
            else
                return _poopsInStock[_poopsInStock.Count - 1];
        }

        public override bool GetIsShowPlace()
        {
            return false;
        }

        public override bool GetCanReceivePoop(PoopBase poopBase = null)
        {
            return _poopsInStock.Count + _waitingPoopAmount < _maxAmountOfPoop;
        }

        public override PoopType GetAcceptedPoopType()
        {
            return PoopType.None;
        }

        public override void CollectPoop(PoopBase poopBase)
        {
            if(_poopsInStock.Contains(poopBase))
                _poopsInStock.Remove(poopBase);
        }
    }
}