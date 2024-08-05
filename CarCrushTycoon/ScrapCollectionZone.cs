using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Chameleon.Game.ArcadeIdle
{
    public class ScrapCollectionZone : MonoBehaviour
    {
        [SerializeField] private ScrapStashBehavior _targetScrapStash;

        public ScrapStashBehavior GetTargetScrapStash()
        {
            return _targetScrapStash;
        }
    }
}