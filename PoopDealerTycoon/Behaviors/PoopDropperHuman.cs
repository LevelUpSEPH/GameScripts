using UnityEngine;
using DG.Tweening;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopDropperHuman : AnimatedPoopDropper
    {
        [SerializeField] private Transform _pooperTransform;
        [SerializeField] private float _poopingShakeDuration = 1f;
        
        protected override void SpawnPoop()
        {
            _pooperTransform.DOShakePosition(_poopingShakeDuration, .1f, 10, 45, false, true, ShakeRandomnessMode.Harmonic).OnComplete(() => StartCoroutine(SpawnPoopAndWait()));
        }

    }
}
