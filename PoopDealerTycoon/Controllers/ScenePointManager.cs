using UnityEngine;
using RocketUtils.SerializableDictionary;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class ScenePointManager : Singleton<ScenePointManager>
    {
        #region POSITION_RELATED
        [SerializeField] private PoopCollectionZonePositionByType _poopCollectionZonePositionByType = new PoopCollectionZonePositionByType(); // pick up from processed/dropped stash
        [SerializeField] private PoopStockPlacePositionByType _poopStockPlacePositionByType = new PoopStockPlacePositionByType(); // for dropping into show places
        [SerializeField] private PoopProcessingStockPlacePositionByType _poopProcessingPlacePositionByType = new PoopProcessingStockPlacePositionByType(); // for dropping into processor
        [SerializeField] private PoopShowPlacePositionByType _poopShowPlacePositionByType = new PoopShowPlacePositionByType(); // for collection
        [SerializeField] private LeavePositionsController _leavePositionController;
       

        public Vector3 GetPoopCollectionZonePositionByType(PoopType poopType) // worker, for processed stash
        {
            return _poopCollectionZonePositionByType[poopType].position;
        }

        public Vector3 GetPoopStockPlacePositionByType(PoopType poopType) // worker, for showplaces
        {
            return _poopStockPlacePositionByType[poopType].position;
        }

        public Vector3 GetProcessingStockPlacePositionByType(PoopType poopType) // worker, for unprocessed stash stock
        {
            return _poopProcessingPlacePositionByType[poopType].position;
        }

        public Vector3 GetPoopShowPlacePositionByType(PoopType poopType) // will be used by customers only
        {
            return _poopShowPlacePositionByType[poopType].GetFreePosition();
        }

        public bool GetPoopShowPlaceHasFreePosition(PoopType poopType)
        {
            if(!_poopShowPlacePositionByType.ContainsKey(poopType))
                return false;
            return _poopShowPlacePositionByType[poopType].GetHasFreePosition();
        }

        public Vector3 GetRandomPositionToLeave()
        {
            return _leavePositionController.GetRandomLeavePosition();
        }
        #endregion POSITION_RELATED

        #region ROTATION_RELATED
        [SerializeField] private Transform _checkoutLookTarget;
        [SerializeField] private PoopStockPlaceLookTargetByType _poopStockPlaceLookTargetByType = new PoopStockPlaceLookTargetByType();
        [SerializeField] private PoopShowplaceLookTargetByType _poopShowplaceLookTargetByType = new PoopShowplaceLookTargetByType();
        [SerializeField] private PoopProcessingStockPlaceLookTargetByType _poopProcessingStockPlaceLookTargetByType = new PoopProcessingStockPlaceLookTargetByType();

        public Transform GetCheckoutTableTransform()
        {
            return _checkoutLookTarget;
        }

        public Transform GetPoopShowplaceLookTarget(PoopType poopType) // for showplace
        {
            return _poopShowplaceLookTargetByType[poopType];
        }

        public Transform GetPoopStockPlaceLookTarget(PoopType poopType) // for taking from stockplace
        {
            return _poopStockPlaceLookTargetByType[poopType];
        }

        public Transform GetPoopProcessingStockPlaceLookTarget(PoopType poopType) // for placing inside stockplace
        {
            return _poopProcessingStockPlaceLookTargetByType[poopType];
        }
        #endregion ROTATION_RELATED

    }

    [System.Serializable]
    public class PoopCollectionZonePositionByType : SerializableDictionary<PoopType, Transform>{}

    [System.Serializable]
    public class PoopStockPlacePositionByType : SerializableDictionary<PoopType, Transform>{}

    [System.Serializable]
    public class PoopProcessingStockPlacePositionByType : SerializableDictionary<PoopType, Transform>{}

    [System.Serializable]
    public class PoopShowPlacePositionByType : SerializableDictionary<PoopType, PoopShowPlacePositionsController>{}

    [System.Serializable]
    public class PoopStockPlaceLookTargetByType : SerializableDictionary<PoopType, Transform>{}

    [System.Serializable]
    public class PoopShowplaceLookTargetByType : SerializableDictionary<PoopType, Transform>{}

    [System.Serializable]
    public class PoopProcessingStockPlaceLookTargetByType : SerializableDictionary<PoopType, Transform>{}

}