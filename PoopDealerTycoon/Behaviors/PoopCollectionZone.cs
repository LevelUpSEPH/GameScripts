using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle.Zones
{
    public class PoopCollectionZone : TriggerZone
    {
        [SerializeField] private StockPlaceBase _targetCollectionPlace = null;

        public StockPlaceBase GetCollectionPlace()
        {
            return _targetCollectionPlace;
        }

    }
}