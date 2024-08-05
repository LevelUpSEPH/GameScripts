using System.Collections;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Helpers;
using DG.Tweening;

namespace Chameleon.Game.ArcadeIdle
{
    public class ProcessedPoopDropper : PoopDropper
    {
        [SerializeField] private PoopStockWithAcceptedType _unprocessedStock;
        [SerializeField] private Transform _animationStartTransform;
        [SerializeField] private Transform _pressMachineTransform;

        protected override bool CanDropPoop()
        {
            return base.CanDropPoop() && !_unprocessedStock.GetIsEmpty();
        }

        protected override void DropPoop()
        {
            if(_unprocessedStock.TryGetSlotWithPoop(out PoopSlot poopSlot))
            {
                _isAnimating = true;
                PoopBase poopInSlot = poopSlot.GetPoopInSlot();
                JumpAnimator.instance.MoveTargetToPosition(poopInSlot.transform, _animationStartTransform.position, .5f, () => OnJumpComplete());
                poopSlot.ClearSlot();
                void OnJumpComplete()
                {
                    StartCoroutine(AnimateAndReleaseNewPoop(poopInSlot));
                }
            }
        }

        private IEnumerator AnimateAndReleaseNewPoop(PoopBase unprocessedPoop)
        {
            yield return new WaitForSeconds(1f);
            _pressMachineTransform.DOLocalMoveY(_pressMachineTransform.localPosition.y -.8f, .5f);
            ScaleAnimator.instance.AnimateScaleDown(unprocessedPoop.transform, .5f, () => AnimateSwap(unprocessedPoop));
        }

        private void AnimateSwap(PoopBase unprocessedPoop)
        {
            unprocessedPoop.DisablePoop();
            string poopTypeString = _productPoopType.ToString();
            GameObject poop = ObjectPool.instance.SpawnFromPool(poopTypeString, _animationStartTransform.position, Quaternion.identity);
            if(poop != null)
            {
                if(poop.activeInHierarchy)
                    _droppedItemsStock.IncrementWaitingPoopAmount();
                PoopBase poopBase = poop.GetComponent<PoopBase>();
                poopBase.SetPhysicsActive(false);
                _pressMachineTransform.DOLocalMoveY(_pressMachineTransform.localPosition.y + .8f, .5f);
                ScaleAnimator.instance.AnimateScaleUpFromZero(poop.transform, .5f, () => ReleaseAnimation(poopBase));
            }
        }

        private void ReleaseAnimation(PoopBase poopToRelease)
        {
            poopToRelease.SetPhysicsActive(true);
            _isAnimating = false;
        }
    }
}