using Game.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class BuildZoneActivationController : Singleton<BuildZoneActivationController> // this script should listen to a savecontroller and initialize after the save is loaded
    {
        public static event Action BuildsLoadingCompleted;
        [SerializeField] private List<BuildableItem> _buildableItems = new List<BuildableItem>();
        private BuildItemSaveData _buildItemSaveData;
        private bool _isInitialized = false;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            if(_isInitialized)
                return;
            _buildItemSaveData = PlayerData.Instance.BuildItemSaveData;
            foreach(BuildableItem buildableItem in _buildableItems)
            {
                BuildItemSaveData.BuildItemData data = null;
                foreach(var buildData in _buildItemSaveData.buildItemDatas)
                {
                    if(buildData.id != buildableItem.GetBuildItemType())
                        continue;
                    data = buildData;
                    break;
                }
                if(data == null)
                {
                    data = new BuildItemSaveData.BuildItemData
                    {
                        id = buildableItem.GetBuildItemType(),
                        requiredMoneyLeft = buildableItem.GetRequiredTotalMoney()
                    };
                    _buildItemSaveData.buildItemDatas.Add(data);
                }
                buildableItem.InitializeWithSaves(data);
                buildableItem.RequiredMoneyChanged += Save;
            }
            Save();
            _isInitialized = true;
            BuildsLoadingCompleted?.Invoke();
        }

        private void Start()
        {
            BuildableItem.BuiltItem += OnBuiltItem;
            if(PlayerData.Instance.IsStartCurrencyAdded)
                ActivateElementsOfCurrentGroup();
            else
                PlayerData.Stats[StatKeys.IsStartCurrencyAdded].Changed += OnStartCurrencyAdded;
        }

        private void OnDisable()
        {
            BuildableItem.BuiltItem -= OnBuiltItem;
            PlayerData.Stats[StatKeys.IsStartCurrencyAdded].Changed -= OnStartCurrencyAdded;
            foreach(BuildableItem buildableItem in _buildableItems)
            {
                buildableItem.RequiredMoneyChanged -= Save;
            }
        }

        private void OnBuiltItem(BuildableItem buildableItem)
        {
            _buildableItems.Remove(buildableItem);
            ActivateElementsOfCurrentGroup();
        }

        private void OnStartCurrencyAdded()
        {
            ActivateElementsOfCurrentGroup();
        }

        private void ActivateElementsOfCurrentGroup()
        {
            if(_buildableItems.Count > 0)
                _buildableItems[0].gameObject.SetActive(true);
        }

        private void Save()
        {
            PlayerData.Instance.SaveBuildItemsSaveData();
        }
    }

}
