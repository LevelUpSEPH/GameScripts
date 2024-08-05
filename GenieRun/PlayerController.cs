using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private int _accuracyLevel = 2;
    private int _accuracyExp = 0;
    
    private int _indicatorPoint = 0;
    private bool _isLockedToIndicator = false;

    private int _requiredExpToLevelUp = 100;

    [SerializeField] private ParticleSystem _positiveParticle;
    [SerializeField] private ParticleSystem _negativeParticle;
    [SerializeField] private PlayerAnimationController _playerAnim;

    public static event Action<int, int> FortuneStatsAreReady;

    private void Awake() {
        FinishTrigger.ParkourFinished += TriggerFortuneStatsAreReady;
    }

    private void OnDisable() {
        FinishTrigger.ParkourFinished -= TriggerFortuneStatsAreReady;
    }

    public void ChangeAccuracyExp(int amount) {
        //HandeCollectableParticle(amount);
        if (_accuracyExp + amount < 0)
            _accuracyExp = 0;
        else
            _accuracyExp += amount;
        CalculateAccuracyLevel();
    }

    private void HandeCollectableParticle(int amount) {
        if (amount > 0)
            _positiveParticle.Play();
        else if(amount < 0)
            _negativeParticle.Play();
    }

    public void ChangeIndicatorPoint(int point) {
        if (_isLockedToIndicator)
            return;

        StartCoroutine(StartDoorCooldown());
        _indicatorPoint += point;
    }

    private void CalculateAccuracyLevel() {
        if(_accuracyExp >= _requiredExpToLevelUp) {
            HandleAccuracyLevelIncrease();
        }
        else if(_accuracyExp <= 0) {
            if (_accuracyLevel == 1)
                return;
            HandleAccuracyLevelDecrease();
        }
    }

    private void HandleAccuracyLevelIncrease() {
        ChangeAccuracyLevel(_accuracyLevel + 1);
        _accuracyExp -= _requiredExpToLevelUp;
        _playerAnim.PlayLevelUpAnimation();
    }

    private void HandleAccuracyLevelDecrease() {
        ChangeAccuracyLevel(_accuracyLevel - 1);
        _accuracyExp += _requiredExpToLevelUp - 1;
    }

    private void ChangeAccuracyLevel(int targetLevel) {
        _accuracyLevel = targetLevel;
    }

    private IEnumerator StartDoorCooldown() {
        _isLockedToIndicator = true;
        yield return new WaitForSeconds(0.5f);
        _isLockedToIndicator = false;
    }

    private void TriggerFortuneStatsAreReady() {
        FortuneStatsAreReady?.Invoke(_indicatorPoint, _accuracyLevel);
    }

    public int GetRequiredExpToLevelUp() {
        return _requiredExpToLevelUp;
    }

    public int GetAccuracyExp() {
        return _accuracyExp;
    }

    public int GetAccuracyLevel() {
        return _accuracyLevel;
    }

    public int GetTotalExp() {
        return ((_accuracyLevel - 1) * _requiredExpToLevelUp) + _accuracyExp;
    }
}
