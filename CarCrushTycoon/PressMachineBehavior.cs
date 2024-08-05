using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;
using DG.Tweening;
using System;

namespace Chameleon.Game.ArcadeIdle.Machine
{
    public class PressMachineBehavior : BaseCrushingMachine
    {
        [SerializeField] private Transform _pressTransform;
        [SerializeField] private float _pressedYValue;
        private float _normalYValue;

        private float _timeToPress = 2f;
        private float _carStartPressOffset = .5f;
        private float _delayBeforeMovingUp = 1f;

        protected override void OnStart()
        {
            _normalYValue = _pressTransform.localPosition.y;
        }

        protected override void PlayDestroyAnimation(Action onCompletedDestroy)
        {
            _pressTransform.DOLocalMoveY(_pressedYValue, _timeToPress).SetEase(Ease.Linear)
                .OnComplete(() => WaitAndMovePressUp());
            
            DOVirtual.DelayedCall(_carStartPressOffset, () => _carToAnimate.transform.DOScaleY(.1f, _timeToPress - _carStartPressOffset));

            DOVirtual.DelayedCall(_timeToPress + _delayBeforeMovingUp + .3f, () => onCompletedDestroy());
        }

        private void WaitAndMovePressUp()
        {
            DOVirtual.DelayedCall(_delayBeforeMovingUp, () => _pressTransform.DOLocalMoveY(_normalYValue, 1).SetEase(Ease.Linear));
        }
    }
}