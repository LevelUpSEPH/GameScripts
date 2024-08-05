using Chameleon.Game.ArcadeIdle.Helpers;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Game.Scripts.Models;
using System;

namespace Chameleon.Game.ArcadeIdle
{
    public class MoneyStackingArea : MonoBehaviour
    {
        public static event Action<int> MoneyCollected;
        [SerializeField] private Transform[] _baseLevelMoneyPositions = new Transform[6];
        [SerializeField] private Transform _moneyRotationTarget;
        [SerializeField] private float _moneyYAxisSize = .2f;
        private List<GameObject> _moneyInStackArea = new List<GameObject>();
        private int _moneyAmountInStock = 0;
        private int _maxPhysicalMoneyAmount = 30;
        private bool _isAnimatingMoneyCollection = false;

        public void AddMoney(int amount)
        {
            int spawnedAmount = 0;
            while(spawnedAmount < amount)
            {
                if(_moneyAmountInStock < _maxPhysicalMoneyAmount)
                {
                    Vector3 moneySpawnPos = _baseLevelMoneyPositions[_moneyAmountInStock % _baseLevelMoneyPositions.Length].position + Vector3.up * Mathf.Floor(_moneyAmountInStock / _baseLevelMoneyPositions.Length) * _moneyYAxisSize;
                    GameObject money = ObjectPool.instance.SpawnFromPool("Money", moneySpawnPos, Quaternion.identity);
                    _moneyInStackArea.Add(money);
                    money.transform.rotation = _moneyRotationTarget.rotation;
                    ScaleAnimator.instance.AnimateScaleUpFromZero(money.transform, .4f);
                }
                spawnedAmount++;
                _moneyAmountInStock++;
            }
        }

        public void CollectMoney(Transform playerTransform)
        {
            if(_moneyAmountInStock <= 0)
                return;
            PlayerData.Instance.EarnCurrency(_moneyAmountInStock);
            MoneyCollected?.Invoke(_moneyAmountInStock);
            _moneyAmountInStock = 0;
            if(!_isAnimatingMoneyCollection)
            {
                StartCoroutine(StartAnimatingMoney(playerTransform));
            }
        }

        private IEnumerator StartAnimatingMoney(Transform playerTransform)
        {
            _isAnimatingMoneyCollection = true;
            for (int i = _moneyInStackArea.Count - 1; i >= 0; i--)
            {
                yield return new WaitForSeconds(.01f);
                GameObject moneyObject = _moneyInStackArea[i];
                JumpAnimator.instance.MoveTargetToPosition(moneyObject.transform, playerTransform.position, .2f, () => {
                    moneyObject.SetActive(false);
                    _moneyInStackArea.Remove(moneyObject);
                });
                if (i == 0)
                    OnMoneyAnimationComplete();
            }
        }

        private void OnMoneyAnimationComplete()
        {
            _isAnimatingMoneyCollection = false;
        }
    }
}