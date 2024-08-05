using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class TutorialArrowBehavior : MonoBehaviour
    {
        private float _initialYPos;

        private void Awake()
        {
            _initialYPos = transform.localPosition.y;
        }

        private void OnEnable()
        {
            ResetPosition();

            StartAnimation();
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        private void ResetPosition()
        {
            transform.localPosition = new Vector3(transform.localPosition.x, _initialYPos, transform.localPosition.z);
        }

        private void StartAnimation()
        {
            transform.DOLocalMoveY(transform.position.y - .5f, 1.25f).SetLoops(-1, LoopType.Yoyo);
        }

        private void StopAnimation()
        {
            transform.DOKill();
        }
    }
}