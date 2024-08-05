using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle.Zones
{
    public class PoopDropZone : TriggerZone
    {
        [SerializeField] private StockPlaceBase _targetDropPlace = null;

        public StockPlaceBase GetPoopDropPlace()
        {
            return _targetDropPlace;
        }

    }
}