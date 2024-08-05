using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketCoroutine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public static class ObjectDisabler
    {
        public static void DisableObjectWithDelay(GameObject targetObject, float delay)
        {
            RocketCoroutine.CoroutineController.StartCoroutine(DisableObject(targetObject, delay));
        }

        private static IEnumerator DisableObject(GameObject targetObject, float delay)
        {
            yield return delay;
            DisableObject(targetObject);
        }

        private static void DisableObject(GameObject targetObject)
        {
            targetObject.SetActive(false);
        }
    }
}