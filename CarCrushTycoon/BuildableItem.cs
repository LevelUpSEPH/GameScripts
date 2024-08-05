using UnityEngine;
using Chameleon.Game.ArcadeIdle.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Chameleon.Game.ArcadeIdle
{
    public class BuildableItem : MonoBehaviour
    {
        public static event Action<BuildableItem> BuiltItem;
        public event Action BuildableItemInitialized;
        [SerializeField] private List<GameObject> _finalItemList = new List<GameObject>();
        [SerializeField] private ParticleSystem _buildCompleteParticle;
        [SerializeField] private int _requiredTotalMoney = 50;
        [SerializeField] private BuildItemType _buildItemType;
        private BuildItemSaveData.BuildItemData _data;
        private int _requiredMoneyLeft;
        private bool _builtItem = false;
        private bool _isInitialized;
        public bool IsInitialized => _isInitialized;

        public event Action RequiredMoneyChanged;

        public void InitializeWithSaves(BuildItemSaveData.BuildItemData buildItemData)
        {
            _requiredMoneyLeft = buildItemData.requiredMoneyLeft;
            _isInitialized = true;
            _data = buildItemData;
            BuildableItemInitialized?.Invoke();
        }

        private void OnEnable()
        {
            StartCoroutine(WaitForInitialize());
        }

        private IEnumerator WaitForInitialize()
        {
            while(true)
            {
                yield return null;
                if(!_isInitialized)
                    continue;
                CheckAndBuildItem(true);
                break;
            }
        }

        public void PayForItem()
        {
            _requiredMoneyLeft--;

            _data.requiredMoneyLeft = _requiredMoneyLeft;

            RequiredMoneyChanged?.Invoke();
            CheckAndBuildItem();
        }

        private void BuildItem(bool isQuickBuildMode = false)
        {
            if(!isQuickBuildMode)
                PlayParticle();
            _builtItem = true;
            foreach(GameObject finalItem in _finalItemList)
            {
                finalItem.SetActive(true);
                if(!isQuickBuildMode)
                    ScaleAnimator.instance.AnimateScaleUpFromZero(finalItem.transform, .5f);
            }
            BuiltItem?.Invoke(this);
            Destroy(gameObject);
        }

        private void CheckAndBuildItem(bool isQuickBuildMode = false)
        {
            if(GetCanBuildItem())
            {
                BuildItem(isQuickBuildMode);
            }
        }

        private bool GetCanBuildItem()
        {
            return !_builtItem && _requiredMoneyLeft <= 0;
        }

        private void PlayParticle()
        {
            if(_buildCompleteParticle != null)
                _buildCompleteParticle.Play();
        }

        public int GetRequiredMoneyLeft()
        {
            return _requiredMoneyLeft;
        }

        public int GetRequiredTotalMoney()
        {
            return _requiredTotalMoney;
        }

        public BuildItemType GetBuildItemType()
        {
            return _buildItemType;
        }

        public float GetBuildPercentage()
        {
            return (float)(_requiredTotalMoney - _requiredMoneyLeft) / _requiredTotalMoney;
        }

        public bool GetCanReceiveMoney()
        {
            return GetRequiredMoneyLeft() > 0;
        }
    }
}