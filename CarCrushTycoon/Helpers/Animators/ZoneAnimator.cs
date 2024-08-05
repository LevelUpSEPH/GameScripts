using UnityEngine;
using DG.Tweening;

namespace Chameleon.Game.ArcadeIdle
{
    public class ZoneAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _borders;

        protected virtual void Start()
        {
            Animate();
        }

        protected virtual void OnDisable()
        {
            StopAnimate();
        }

        protected virtual void Animate()
        {
            AnimateBorders();
        }

        private void AnimateBorders()
        {
            StartSequence();
        }

        private void StartSequence()
        {
            ScaleUp();
        }

        private void ScaleUp()
        {
            _borders.DOScale(1.07f, .4f).SetEase(Ease.Linear).OnComplete(() => ScaleDown());
        }

        private void ScaleDown()
        {
            _borders.DOScale(1f, .4f).SetEase(Ease.Linear).OnComplete(() => ScaleUp());
        }

        protected virtual void StopAnimate()
        {
            _borders.DOKill();
        }

        protected void SetBordersActive(bool isActive)
        {
            _borders.gameObject.SetActive(isActive);
        }

    }
}