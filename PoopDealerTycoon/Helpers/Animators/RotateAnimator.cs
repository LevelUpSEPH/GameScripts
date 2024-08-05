using UnityEngine;
using System;
using DG.Tweening;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class RotateAnimator : Singleton<RotateAnimator>
    {
        public void RotateTowards(Transform rotating, Transform rotateTarget, float duration, Action onComplete = null)
        {
            Vector3 rotatingPosition = rotating.position;
            Vector3 rotateTargetPosition = new Vector3(rotateTarget.position.x, rotatingPosition.y, rotateTarget.position.z);
            Vector3 dirToTarget = rotateTargetPosition - rotatingPosition;
            Quaternion rotationtoTarget = Quaternion.LookRotation(dirToTarget);
            if(onComplete != null)
                rotating.DORotateQuaternion(rotationtoTarget, duration).OnComplete(() => onComplete()).SetEase(Ease.OutQuad);
            else
                rotating.DORotateQuaternion(rotationtoTarget, duration).SetEase(Ease.OutQuad);
        }
    }

}
