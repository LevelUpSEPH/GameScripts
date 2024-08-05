using System;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class BoxAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _boxCover;
        [SerializeField] private Transform _boxCoverFinalPosition;
        private Vector3 _initialLocalPosition;

        private void Awake()
        {
            _initialLocalPosition = _boxCover.transform.localPosition;
        }

        private void OnEnable()
        {
            _boxCover.transform.localPosition = _initialLocalPosition;
        }

        public void RemoveParent()
        {
            ParentRemover.instance.RemoveParentFrom(transform);
            gameObject.SetActive(false);
        }

        public void AnimateCover(Action onComplete)
        {
            JumpAnimator.instance.MoveTargetToPosition(_boxCover, _boxCoverFinalPosition.position, .4f, onComplete);
        }



    }
}