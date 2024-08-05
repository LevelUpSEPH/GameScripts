using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    [CreateAssetMenu(fileName = "CarStat", menuName = "ScriptableObjects/CarStats", order = 1)]
    public class CarStats : ScriptableObject
    {
        public float maxSteeringAngle;
        public float maxSpeed;
        public float acceleration;

        public ScrapWorth scrapWorth;
    }

    [System.Serializable]
    public struct ScrapWorth
    {
        public int cheapScrapWorth;
        public int midScrapWorth;
        public int expensiveScrapWorth;
    }
}