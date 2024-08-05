using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public abstract class StockPlaceBase : MonoBehaviour, ICollectionPlace, IDropPlace
    {
        public abstract PoopType GetAcceptedPoopType();

        public abstract bool GetCanReceivePoop(PoopBase poopBase = null);

        public abstract PoopBase GetExamplePoop();

        public abstract bool GetIsShowPlace();

        public abstract bool TryDropPoop(PoopBase poopBase);
        public abstract void CollectPoop(PoopBase poopBase);
    }
}