using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    [CreateAssetMenu(fileName = "Poop", menuName = "ScriptableObjects/PoopTypes", order = 1)]
    public class PoopScriptableObject : ScriptableObject
    {
        public PoopType poopType;
        public bool isBaseOfSet;
    }
}
