using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class PoopShowPlacePositionsController : MonoBehaviour
    {
        [SerializeField] private List<PoopShowPlacePosition> _poopShowPlacePositions;
        private Dictionary<PoopShowPlacePosition, bool> _waitingPositionAvailabilityDict = new Dictionary<PoopShowPlacePosition, bool>();

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            foreach(PoopShowPlacePosition position in _poopShowPlacePositions)
            {
                position.PlaceAvailabilityChanged -= OnPlaceAvailabilityChanged;
            }
        }

        private void Initialize()
        {
            foreach(PoopShowPlacePosition poopShowPlacePosition in _poopShowPlacePositions)
            {
                _waitingPositionAvailabilityDict.Add(poopShowPlacePosition, true);
                poopShowPlacePosition.PlaceAvailabilityChanged += OnPlaceAvailabilityChanged;
            }
        }

        public Vector3 GetFreePosition()
        {
            foreach(var key in _waitingPositionAvailabilityDict.Keys)
            {
                if(_waitingPositionAvailabilityDict[key] == true)
                {
                    _waitingPositionAvailabilityDict[key] = false;
                    return key.transform.position;
                }
            }
            return Vector3.zero;
        }

        public bool GetHasFreePosition()
        {
            foreach(var key in _waitingPositionAvailabilityDict.Keys)
            {
                if(_waitingPositionAvailabilityDict[key] == true)
                    return true;
            }
            return false;
        }

        private void OnPlaceAvailabilityChanged(PoopShowPlacePosition showPlacePosition, bool isAvailable)
        {
            _waitingPositionAvailabilityDict[showPlacePosition] = isAvailable;
        }
    }

}