using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class ScaleAnimator : Singleton<ScaleAnimator>
    {
        public void AnimateScaleUpFromZero(Transform targetTransform, float duration, Action onComplete = null)
        {
            Vector3 initialScale = targetTransform.localScale;
            if(onComplete != null)
            {
                targetTransform.localScale = Vector3.one * .1f;
                targetTransform.DOScale(initialScale, duration)
                .OnComplete(() => onComplete());
            }
            else
            {
                targetTransform.localScale = Vector3.one * .1f;
                targetTransform.DOScale(initialScale, duration);
            }

        }

        public void AnimateScaleDown(Transform targetTransfrom, float duration, Action onComplete = null)
        {
            if(onComplete != null)
            {
                targetTransfrom.DOScale(Vector3.one * .1f, duration)
                .OnComplete(() => onComplete())
                .SetEase(Ease.Linear);
            }
            else
            {
                targetTransfrom.DOScale(Vector3.one * .1f, duration)
                .SetEase(Ease.Linear);
            }
        }

        public void AnimateScaleTo(Transform targetTransform, Vector3 targetScale, float duration, Action onComplete = null)
        {
            targetTransform.DOKill();
            if(onComplete != null)
            {
                targetTransform.DOScale(targetScale, duration)
                .OnComplete(() => onComplete())
                .SetEase(Ease.Linear);
            }
            else
            {
                targetTransform.DOScale(targetScale, duration)
                .SetEase(Ease.Linear);
            }
        }

    }
}
