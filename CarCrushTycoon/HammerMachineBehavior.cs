using Chameleon.Game.ArcadeIdle.Abstract;
using System;
using DG.Tweening;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Machine
{
    public class HammerMachineBehavior : BaseCrushingMachine
    {
        [SerializeField] private Transform _leftHammer; // left means *-1
        [SerializeField] private Transform _rightHammer; // right means *1

        [SerializeField] private float _hammerXAngleToHit = 10;

        [SerializeField] private ParticleSystem _leftHitParticle;
        [SerializeField] private ParticleSystem _rightHitParticle;

        private Vector3 _initialHammerRotation;

        private float _hittingDuration = .25f;
        private float _returningToNormalDuration = 1f;

        protected override void OnStart()
        {
            _initialHammerRotation = _rightHammer.localRotation.eulerAngles;
        }

        protected override void PlayDestroyAnimation(Action onCompletedDestroy)
        {
            HitWithConsecutiveHammers();

            HitWithBothHammersAndComplete(onCompletedDestroy);
        }

        private void HitWithConsecutiveHammers()
        {
            HitHammerAndReturnToNormal(_leftHammer, true, () => _carToAnimate.DOPunchScale(-Vector3.one * .2f, .5f));

            DOVirtual.DelayedCall(_hittingDuration + _returningToNormalDuration, () => HitHammerAndReturnToNormal(_rightHammer, false, () => _carToAnimate.DOPunchScale(-Vector3.one * .2f, .5f)));
        }

        private void HitWithBothHammersAndComplete(Action onComplete)
        {
            DOVirtual.DelayedCall((_hittingDuration + _returningToNormalDuration) * 2, () => 
            {
                HitHammerAndReturnToNormal(_leftHammer, true);
                HitHammerAndReturnToNormal(_rightHammer, false, onComplete);
            });
        }

        private void PlayHitParticle(bool isLeftHammer)
        {
            if(isLeftHammer)
            {
                _leftHitParticle.Play();
            }
            else
            {
                _rightHitParticle.Play();
            }
        }

        private void HitHammerAndReturnToNormal(Transform hammerToHit, bool isLeftHammer, Action onHitComplete = null)
        {
            int positivityMultiplier = 1;
            if(isLeftHammer)
                positivityMultiplier = -1;
            
            Vector3 hammerTargetRotation = new Vector3(_hammerXAngleToHit * positivityMultiplier, hammerToHit.localRotation.y, hammerToHit.localRotation.z);
            Vector3 initialRotation = new Vector3(_initialHammerRotation.x * positivityMultiplier, _initialHammerRotation.y, _initialHammerRotation.z);
            
            hammerToHit.DOLocalRotate(hammerTargetRotation, _hittingDuration, RotateMode.Fast).SetEase(Ease.InQuart)
                .OnComplete(() => 
                {
                    PlayHitParticle(isLeftHammer);
                    hammerToHit.DOLocalRotate(initialRotation, _returningToNormalDuration);
                    if(onHitComplete != null)
                        onHitComplete();
                })
                .SetEase(Ease.Linear);
        }
    }
}