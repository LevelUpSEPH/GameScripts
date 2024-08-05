using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.Animation
{
    public class SimpleAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SetMoveAnimationPlaying(bool isMoving){
            _animator.SetBool("IsMoving", isMoving);
        }

        public void SetCarryingAnimationPlaying(bool isCarrying)
        {
            _animator.SetBool("IsCarrying", isCarrying);
        }

    }
}