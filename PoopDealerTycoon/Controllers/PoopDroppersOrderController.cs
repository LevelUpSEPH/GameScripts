using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopDroppersOrderController : MonoBehaviour
    {
        public static event Action DroppersActivated;
        [SerializeField] private List<AnimatedPoopDropper> _poopDroppers = new List<AnimatedPoopDropper>();
        private bool _isPoopAnimating = false;
        private int _poopingIndex = 0;

        private void Start()
        {
            StartCoroutine(PoopingSequence());
            DroppersActivated?.Invoke();
        }

        private void OnDisable()
        {
            foreach(AnimatedPoopDropper poopDropper in _poopDroppers)
            {
                poopDropper.PoopingSequenceCompleted -= OnCurrentPoopingSequenceComplete;   
            }
        }

        private IEnumerator PoopingSequence()
        {
            while(true)
            {
                if(_isPoopAnimating)
                {
                    yield return .1f;
                    continue;
                }
                _isPoopAnimating = true;
                AnimatedPoopDropper nextPoopDropper = _poopDroppers[_poopingIndex % _poopDroppers.Count];
                nextPoopDropper.SetTurnToPoop(true);
                nextPoopDropper.PoopingSequenceCompleted += OnCurrentPoopingSequenceComplete;
                yield return null;
            }
        }

        private void OnCurrentPoopingSequenceComplete()
        {
            _poopDroppers[_poopingIndex % _poopDroppers.Count].PoopingSequenceCompleted -= OnCurrentPoopingSequenceComplete;
            _poopingIndex++;
            _isPoopAnimating = false;
        }
    }
}