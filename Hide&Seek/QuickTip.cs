using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class QuickTip : MonoBehaviour
{
    public static event Action TipClosed;

    [SerializeField] private GameObject _visualsPanel;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private Button _closeButton;

    private void Start()
    {
        _closeButton.onClick.AddListener(CloseTip);
        InLevelController.LevelCompleted += OnLevelCompleted;
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveAllListeners();
        InLevelController.LevelCompleted -= OnLevelCompleted;
    }

    private void OnLevelCompleted(bool unused)
    {
        CloseTip();
    }

    public void ShowTip(string tip)
    {
        _visualsPanel.SetActive(true);
        //_tipText.text = tip;
    }

    private void CloseTip()
    {
        _visualsPanel.SetActive(false);
        TipClosed?.Invoke();
    }

}
