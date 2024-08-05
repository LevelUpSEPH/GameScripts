using Chameleon.Game.ArcadeIdle.Abstract;
using UnityEngine;
using System;
using DG.Tweening;

namespace Chameleon.Game.ArcadeIdle.Machine
{
    public class FireMachineBehavior : BaseCrushingMachine
    {
        [SerializeField] private ParticleSystem[] _fireParticles;
        protected override void PlayDestroyAnimation(Action onCompletedDestroy)
        {
            SetFireParticlePlaying(true);

            float timeToTurnDark = 3f;
            TurnCarDarker(timeToTurnDark);

            DOVirtual.DelayedCall(3, () => SetFireParticlePlaying(false));

            DOVirtual.DelayedCall(4, () => onCompletedDestroy());
        }

        private void TurnCarDarker(float duration)
        {
            MeshRenderer[] meshRenderers = _carToAnimate.GetComponentsInChildren<MeshRenderer>();

            foreach(MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material.DOColor(Color.black, 2f);
            }
        }

        private void SetFireParticlePlaying(bool isPlaying)
        {
            if(isPlaying)
            {
                foreach(ParticleSystem fireParticle in _fireParticles)
                    fireParticle.Play();
            }
            else
            {
                foreach(ParticleSystem fireParticle in _fireParticles)
                    fireParticle.Stop();
            }
        }
    }
}