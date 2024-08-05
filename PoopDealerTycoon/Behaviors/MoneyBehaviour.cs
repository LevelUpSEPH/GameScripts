using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class MoneyBehaviour : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 1f;
        private void OnEnable()
        {
            StartCoroutine(Lifetime());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator Lifetime()
        {
            
            yield return new WaitForSeconds(_lifeTime);
            gameObject.SetActive(false);
        }
    }
}