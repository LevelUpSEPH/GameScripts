using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketUtils.SerializableDictionary;
using System;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class PoopTypeActivityController : Singleton<PoopTypeActivityController>
    {
        public static event Action<PoopType> NewPoopTypeActivated;
        [SerializeField] private ActivityBySet _activityBySet = new ActivityBySet();


        public void SetIsPoopTypeActive(PoopType poopType, bool isActive)
        {
            PoopSet poopSet = PoopSetByType.GetPoopSetByType(poopType);
            _activityBySet[poopSet][poopType] = isActive;
            NewPoopTypeActivated?.Invoke(poopType);
        }
        
        public bool GetIsPoopTypeActive(PoopType poopType)
        {
            PoopSet poopSet = PoopSetByType.GetPoopSetByType(poopType);
            return _activityBySet[poopSet][poopType];
        }

        public int GetActivePoopTypeCount()
        {
            return GetActiveTypes().Count;
        }

        public List<PoopType> GetActiveTypes()
        {
            List<PoopType> activeTypes = new List<PoopType>();
            foreach(var setKey in _activityBySet.Keys)
            {
                ActivityByPoopType activityByPoopType = _activityBySet[setKey];
                foreach(var typeKey in activityByPoopType.Keys)
                if(activityByPoopType[typeKey])
                    activeTypes.Add(typeKey);
            }
            return activeTypes;
        }

        public List<PoopType> GetActiveTypesBySet(PoopSet targetSet)
        {
            List<PoopType> activeTypes = new List<PoopType>();
            ActivityByPoopType activityByPoopType = _activityBySet[targetSet];
            foreach(var key in activityByPoopType.Keys)
            {
                if(activityByPoopType[key])
                    activeTypes.Add(key);
            }
            return activeTypes;
        }

        [System.Serializable]
        public class ActivityByPoopType : SerializableDictionary<PoopType, bool>{}

        [System.Serializable]
        public class ActivityBySet : SerializableDictionary<PoopSet, ActivityByPoopType>{}
    }
}
