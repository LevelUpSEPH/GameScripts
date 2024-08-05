using UnityEngine;
using System;
using System.Collections;

namespace Chameleon.Game.ArcadeIdle
{
    public class AnimatedPoopDropper : PoopDropper
    {
        public event Action PoopingSequenceCompleted;

        [SerializeField] protected PoopDropperAnimationController _poopDropperAnimationController;
        [SerializeField] protected float _animationSpeedsMultiplier = 1f;
        [SerializeField] protected float _delayToGetUp = .5f;
        [SerializeField] private ParticleSystem _poopingParticle;
        protected bool _isMyTurnToPoop = false;

        protected override void OnDisable()
        {
            base.OnDisable();

            _poopDropperAnimationController.CrouchComplete -= OnCrouchComplete;
            _poopDropperAnimationController.GetUpComplete -= OnGetUpComplete;
        }

        protected override void DropPoop()
        {
            if(_isAnimating)
                return;
            _isAnimating = true;
            BeginPoopingRoutine();
        }

        private void BeginPoopingRoutine()
        {
            PlayCrouchAnimation();
        }

        private void PlayCrouchAnimation()
        {
            _poopDropperAnimationController.PlayCrouchAnimation(_animationSpeedsMultiplier);
            _poopDropperAnimationController.CrouchComplete += OnCrouchComplete;
        }

        protected override bool CanDropPoop()
        {
            return base.CanDropPoop() &&
            _isMyTurnToPoop;
        }

        protected virtual void OnCrouchComplete()
        {
            _poopDropperAnimationController.CrouchComplete -= OnCrouchComplete;
            SpawnPoop();
        }

        protected void PlayGetUpAnimation()
        {
            _poopDropperAnimationController.PlayGetUpAnimation(_animationSpeedsMultiplier);
            _poopDropperAnimationController.GetUpComplete += OnGetUpComplete;
        }

        private void OnGetUpComplete()
        {
            _poopDropperAnimationController.GetUpComplete -= OnGetUpComplete;
            _isAnimating = false;
            _isMyTurnToPoop = false;
        }

        public void SetTurnToPoop(bool isMyTurn)
        {
            _isMyTurnToPoop = isMyTurn;
        }

        protected void InvokePoopSequenceComplete()
        {
            PoopingSequenceCompleted?.Invoke();
        }

        protected virtual void SpawnPoop()
        {

        }

        protected IEnumerator SpawnPoopAndWait()
        {
            _poopingParticle.Play();
            GameObject poop = ObjectPool.instance.SpawnFromPool(_productPoopType.ToString(), _poopInitialPos.position, Quaternion.identity);
            if(poop != null)
                if(poop.activeInHierarchy)
                    _droppedItemsStock.IncrementWaitingPoopAmount();
            InvokePoopSequenceComplete();
            yield return new WaitForSeconds(_delayToGetUp);
            PlayGetUpAnimation();
        }

    }
}