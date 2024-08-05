using UnityEngine;
using DG.Tweening;

namespace Chameleon.Game.ArcadeIdle
{
    public class BuildZoneAnimatorWithArrow : ZoneAnimator
    {
        [SerializeField] private Transform _arrow;

        protected override void Animate()
        {
            base.Animate();
            AnimateArrow();
        }

        protected override void StopAnimate()
        {
            base.StopAnimate();
            _arrow.DOKill();
        }

        private void AnimateArrow()
        {
            GoDown();
            void GoDown()
            {
                _arrow.DOMoveY(_arrow.position.y - .35f, 1f).SetEase(Ease.Linear).OnComplete(() => GoUp());
            }
            void GoUp()
            {
                _arrow.DOMoveY(_arrow.position.y + .35f, 1f).SetEase(Ease.Linear).OnComplete(() => GoDown());
            }
        }
    }
}