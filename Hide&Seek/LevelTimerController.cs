using System.Collections;
using UnityEngine;
using System;

namespace Chameleon.Game.Scripts.Timer
{
    public class LevelTimerController : Singleton<LevelTimerController>
    {
        public static event Action RanOutOfTime;

        private float _remainingTime = 10f;
        private float _levelTime = 10f;
        private bool _isInfiniteTimeLevel = false;
        public bool IsTimerActive => _remainingTime > 0;

        private void Start()
        {
            LevelBehaviour.Started += OnLevelStarted;
            InLevelController.GameStarted += OnGameStarted;
            InLevelController.LevelCompleted += OnLevelCompleted;
        }

        private void OnDisable()
        {
            LevelBehaviour.Started -= OnLevelStarted;
            InLevelController.GameStarted -= OnGameStarted;
            InLevelController.LevelCompleted -= OnLevelCompleted;
        }

        private void OnLevelStarted()
        {
            _isInfiniteTimeLevel = LevelController.instance.CurrentLevel.IsInfiniteTimeLevel;
            if(_isInfiniteTimeLevel)
            {
                _remainingTime = 0;
            }
            else
            {
                _levelTime = LevelController.instance.CurrentLevel.LevelTime;
                _remainingTime = _levelTime;
            }
        }

        private void OnGameStarted()
        {
            if(!_isInfiniteTimeLevel)
                StartLevelTimer();
        }

        private void OnLevelCompleted(bool unused)
        {
            StopAllCoroutines();
        }

        private void StartLevelTimer()
        {
            StartCoroutine(StartLevelCountdown());
        }

        private IEnumerator StartLevelCountdown()
        {
            while(true)
            {
                _remainingTime -= Time.deltaTime;
                yield return null;
                if(_remainingTime <= 0)
                {
                    RunOutOfTime();
                    break;
                }
            }
        }

        private void RunOutOfTime()
        {
            RanOutOfTime?.Invoke();
        }

        public float GetRemainingTime()
        {
            return _remainingTime;
        }

        public float GetRemainingTimePercentage()
        {
            return _remainingTime / _levelTime;
        }
    }

}