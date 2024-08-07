using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitLevelUI : MonoBehaviour
{
    private int _unitLevel;
    private PlayerController _playerController;
    [SerializeField] private Unit _unit;
    [SerializeField] private TextMeshProUGUI _levelDisplay;
    [SerializeField] private Transform _textTransform;
    private Camera _mainCamera;

    private void Awake(){
        GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
        _playerController = playerRef.GetComponent<PlayerController>();
        _unitLevel = _unit.GetLevel();

        EnemyController.EnemyGotScared += OnEnemyGotScared;
    }

    private void Start(){
        _mainCamera = Camera.main;
        UpgradeItem.AnyUpgradeReceived += UpdateLevelTexts;
        UpdateLevelTexts();
    }

    private void OnDisable() {
        UpgradeItem.AnyUpgradeReceived -= UpdateLevelTexts;
        EnemyController.EnemyGotScared -= OnEnemyGotScared;
    }

    private void Update(){
        //_textTransform.transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward, _mainCamera.transform.rotation * Vector3.up);
        if(_playerController.GetIsPlayerHiding())
            HideUI();
        else
            ShowUI();
    }

    private void LateUpdate() {
        _textTransform.transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward, _mainCamera.transform.rotation * Vector3.up);
    }

    private void UpdateLevelTexts(){
        _unitLevel = _unit.GetLevel();
        if(_unit == _playerController)
            _levelDisplay.color = Color.white;
        else if(_unitLevel > _playerController.GetLevel())
            _levelDisplay.color = Color.red;
        else
            _levelDisplay.color = Color.green;
        _levelDisplay.text = "Lv. " + _unitLevel.ToString();
    }

    private void HideUI(){
        _levelDisplay.enabled = false;
    }

    private void ShowUI(){
        _levelDisplay.enabled = true;
    }

    private void OnEnemyGotScared(Unit unit) {
        if (unit == _unit) {;
            _levelDisplay.gameObject.SetActive(false);
        }            
    }
}
