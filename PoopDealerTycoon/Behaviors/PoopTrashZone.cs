using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Zones
{
    public class PoopTrashZone : TriggerZone
    {
        [SerializeField] private PoopTrasher _poopTrasher = null;

        public PoopTrasher GetPoopTrasher()
        {
            return _poopTrasher;
        }
    }
}