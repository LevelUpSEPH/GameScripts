using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.Animation;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public class BaseMovementController : MonoBehaviour
    {
        protected bool _isMoving;
        private SimpleAnimationController _animationController;

        private void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            if(TryGetComponent<SimpleAnimationController>(out SimpleAnimationController animationController))
                _animationController = animationController;
        }

        protected void SetMoveAnimationPlaying(bool isMoving)
        {
            _animationController.SetMoveAnimationPlaying(isMoving);
        }

        public bool GetIsMoving()
        {
            return _isMoving;
        }       
    }
}