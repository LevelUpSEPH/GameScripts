using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class JumpAnimator : Singleton<JumpAnimator>
    {
        public void MoveTargetToPosition(Transform targetTransform, Vector3 finalPosition, float duration = .5f, Action onComplete = null)
        {
            float jumpPower = 2f;
            int jumpAmount = 1;
            if(onComplete != null)
                targetTransform.DOJump(finalPosition, jumpPower, jumpAmount, duration)
                .OnComplete(() => onComplete()); // does not follow 
            else
                targetTransform.DOJump(finalPosition, jumpPower, jumpAmount, duration); // does not follow
        }

        public void MoveTargetToPosition(Transform targetTransform, Vector3 finalPosition, float duration = .5f, float jumpPower = 2, Action onComplete = null)
        {
            int jumpAmount = 1;
            if(onComplete != null)
                targetTransform.DOJump(finalPosition, jumpPower, jumpAmount, duration)
                .OnComplete(() => onComplete()); // does not follow 
            else
                targetTransform.DOJump(finalPosition, jumpPower, jumpAmount, duration); // does not follow 
        }
    }
}