using UnityEngine;
using System.Collections;
using Chameleon.Game.Scripts.Controller;

namespace Chameleon.Game.Scripts.View
{
    public class FrogView : MonoBehaviour
    {
        [SerializeField] private Animator _frogAnimator;
        [SerializeField] private FrogGridContent _targetFrog;
        [SerializeField] private SkinnedMeshRenderer _frogRenderer;
        
        [SerializeField] private float _blinkInterval = 4f;
        [SerializeField] private float _blinkAnimationLength = 1f;

        private Coroutine _blinkRoutine;

        private void OnEnable()
        {
            _blinkRoutine = StartCoroutine(BlinkRoutine());

            RegisterEvents();
        }

        private void OnDisable()
        {
            if(_blinkRoutine != null)
            {
                StopCoroutine(_blinkRoutine);
                _blinkRoutine = null;
            }
            
            UnregisterEvents();
        }

        private IEnumerator BlinkRoutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(_blinkInterval);
                Blink();
            }
        }

        private void Blink()
        {
            StartCoroutine(AnimateBlinkRoutine());
        }

        private IEnumerator AnimateBlinkRoutine()
        {
            float blendShapeWeight = 0f;
            float changeAmount = (200 / _blinkAnimationLength) * .01f;

            while(blendShapeWeight < 99)
            {
                blendShapeWeight += changeAmount;
                SetFrogBlendShapeWeight(blendShapeWeight);
                yield return new WaitForSeconds(0.01f);
            }
            while(blendShapeWeight > 1)
            {
                blendShapeWeight -= changeAmount;
                SetFrogBlendShapeWeight(blendShapeWeight);
                yield return new WaitForSeconds(0.01f);
            }
        }

        private void PlayFlingTongueAnimation()
        {
            _frogAnimator.SetTrigger("FlingTongue");
        }

        private void PlayEatAnimation()
        {
            _frogAnimator.SetTrigger("Eat");
        
        }

        private void PlayReturnToIdleAnimation()
        {
            _frogAnimator.SetTrigger("ReturnToIdle");
        
        }

        private void SetFrogBlendShapeWeight(float blendShapeWeight)
        {
            _frogRenderer.SetBlendShapeWeight(0, blendShapeWeight);
        }

        private void RegisterEvents()
        {
            _targetFrog.AteFruit += PlayEatAnimation;
            _targetFrog.FlungTongue += PlayFlingTongueAnimation;
            _targetFrog.ReturnToIdle += PlayReturnToIdleAnimation;
        }

        private void UnregisterEvents()
        {
            _targetFrog.AteFruit -= PlayEatAnimation;
            _targetFrog.FlungTongue -= PlayFlingTongueAnimation;
            _targetFrog.ReturnToIdle -= PlayReturnToIdleAnimation;
        }
    }
}