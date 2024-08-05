using System;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class BuildZone : MonoBehaviour
    {
        [SerializeField] private BuildableItem _buildableItem;

        public BuildableItem GetBuildableItem()
        {
            return _buildableItem;
        }
    }
}
