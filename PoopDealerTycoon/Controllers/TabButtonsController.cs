using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chameleon.Game.ArcadeIdle
{
    public class TabButtonsController : MonoBehaviour
    {
        [SerializeField] private Button _openWorkersTabButton;
        [SerializeField] private Button _openItemsTabButton;
        [SerializeField] private GameObject _workersTab;
        [SerializeField] private GameObject _itemsTab;
        private GameObject _activeTab = null;

        private void Start()
        {
            _openWorkersTabButton.onClick.AddListener(OnClickedWorkersButton);
            _openItemsTabButton.onClick.AddListener(OnClickedItemsButton);
        }

        private void OnEnable()
        {
            _activeTab = _workersTab;
            OpenActiveTab();
        }

        private void OnDisable()
        {
            _activeTab.SetActive(false);
            _activeTab = null;
        }

        private void OnDestroy()
        {
            _openWorkersTabButton.onClick.RemoveListener(OnClickedWorkersButton);
            _openItemsTabButton.onClick.RemoveListener(OnClickedItemsButton);
        }

        private void OnClickedWorkersButton()
        {
            SwitchToTab(_workersTab);
        }

        private void OnClickedItemsButton()
        {
            SwitchToTab(_itemsTab);
        }

        private void SwitchToTab(GameObject targetTab)
        {
            if(targetTab == _activeTab)
                return;
            else
            {
                _activeTab.gameObject.SetActive(false);
                _activeTab = targetTab;
                OpenActiveTab();
            }
        }

        private void OpenActiveTab()
        {
            _activeTab.SetActive(true);
        }

    }
}