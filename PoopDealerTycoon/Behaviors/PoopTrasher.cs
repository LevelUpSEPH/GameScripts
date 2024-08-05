using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopTrasher : MonoBehaviour
    {
        [SerializeField] private Transform _poopFinalPositionTransform;
        public void TrashPoop(PoopBase poop)
        {
            poop.MoveToPosition(_poopFinalPositionTransform.position, DestroyPoop);
            void DestroyPoop()
            {
                poop.DisablePoop();
            }
        }
    }
}