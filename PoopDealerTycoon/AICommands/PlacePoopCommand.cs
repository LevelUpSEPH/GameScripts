using UnityEngine;
using Chameleon.Game.ArcadeIdle.Helpers;

namespace Chameleon.Game.ArcadeIdle.Commands
{
    public class PlacePoopCommand : PlaceCommand // place on processing stock
    {
        protected override Vector3 GetTargetPositionWithType(PoopType poopType)
        {
            return ScenePointManager.instance.GetProcessingStockPlacePositionByType(poopType);
        }

        protected override Transform GetLookTargetTransform()
        {
            return ScenePointManager.instance.GetPoopProcessingStockPlaceLookTarget(_poopTypeToTake);
        }
    }
}
