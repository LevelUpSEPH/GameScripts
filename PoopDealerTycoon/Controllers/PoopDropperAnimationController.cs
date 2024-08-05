using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Chameleon.Game.ArcadeIdle
{

    public class PoopDropperAnimationController : MonoBehaviour
    {
        public event Action CrouchComplete;
        public event Action GetUpComplete;
        private Animator _pooperAnimator;

        private void Start()
        {
            _pooperAnimator = GetComponent<Animator>();
        }

        public void PlayCrouchAnimation(float playbackMultiplier)
        {
            _pooperAnimator.SetFloat("Crouch", playbackMultiplier);
            _pooperAnimator.SetFloat("GetUp", 0);
        }

        public void PlayGetUpAnimation(float playbackMultiplier)
        {
            _pooperAnimator.SetFloat("GetUp", playbackMultiplier);
            _pooperAnimator.SetFloat("Crouch", 0);
        }

        public void InvokeCrouchComplete()
        {
            CrouchComplete?.Invoke();
        }

        public void InvokeGetUpComplete()
        {
            GetUpComplete();
        }
    }
}