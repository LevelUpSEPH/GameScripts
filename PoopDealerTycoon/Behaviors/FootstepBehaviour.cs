using System.Collections;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Movement
{
    public class FootstepBehaviour : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _stepParticle;
        [SerializeField] private float _lifeTime = 3f;
        public void StartLifetime()
        {
            _stepParticle.Play();
            var main = _stepParticle.main;
            main.startLifetime = 3f;
            StartCoroutine(DisableAfterLifetime());
        }

        private IEnumerator DisableAfterLifetime()
        {
            yield return new WaitForSeconds(_lifeTime);
            gameObject.SetActive(false);
        }
    }
}