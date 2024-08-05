using System.Collections;
using System.Collections.Generic;
using Chameleon.Game.ArcadeIdle.Helpers;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Commands
{
    public class PlacePoopOnShowCommand : PlaceCommand
    {
        protected override Vector3 GetTargetPositionWithType(PoopType poopType)
        {
            return ScenePointManager.instance.GetPoopStockPlacePositionByType(poopType);
        }

        protected override Transform GetLookTargetTransform()
        {
            return ScenePointManager.instance.GetPoopShowplaceLookTarget(_poopTypeToTake);
        }
    }
}