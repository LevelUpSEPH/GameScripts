using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedTowerControllerUI : Singleton<SelectedTowerControllerUI>
{
    [Header("Tower Stat Views")]
    [SerializeField] private TextMeshProUGUI _towerHeader; // tower stats can all be listed one after the other, can also show range of tower on click
    [SerializeField] private TextMeshProUGUI _towerDamage;
    [SerializeField] private TextMeshProUGUI _towerAttackSpeed;
    [SerializeField] private TextMeshProUGUI _towerRange;

    [Header("Other UI Elements")]
    [SerializeField] private GameObject _visualsParent;

    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TextMeshProUGUI _upgradeButtonText;
    [SerializeField] private Button _sellButton;
    [SerializeField] private TextMeshProUGUI _sellButtonText;

    private TowerBase _selectedTower;
    private bool _isTowerSelected = false;

     protected override void Awake(){
        base.Awake();

        _upgradeButton.onClick.AddListener(() => ClickedUpgradeButton());
        _sellButton.onClick.AddListener(() => ClickedDeleteButton());
    }

    private void OnDisable(){
        
        _upgradeButton.onClick.RemoveListener(() => ClickedUpgradeButton());
        _sellButton.onClick.RemoveListener(() => ClickedDeleteButton());
    }

    private void OnClickedCloseTabButton(){
        DisableVisuals();
    }

    public void DisplaySelectedTowerInfo(TowerBase tower){
        _selectedTower = tower;
        UpdateUIPosition();

        EnableVisuals();

        if(tower != null)
            SetUIElements();
        else
            _towerHeader.text = "No Tower";
    }

    private void SetUIElements(){
        TowerSO nextLevelTower = _selectedTower.GetTowerScriptableObject().nextLevelTower;

        _towerHeader.text = _selectedTower.GetTowerName();

        _towerHeader.text = _towerHeader.text + " - Lv." + _selectedTower.GetTowerLevel().ToString();

        _towerDamage.text = _selectedTower.GetTowerDamage().ToString();
        if(nextLevelTower != null)
            _towerDamage.text += " -> " + nextLevelTower.shotDamage;

        _towerAttackSpeed.text = _selectedTower.GetTowerShootingCD().ToString();
        if(nextLevelTower != null)
            _towerAttackSpeed.text += " -> " + nextLevelTower.shootingCD;

        _towerRange.text = _selectedTower.GetTowerRange().ToString();
        if(nextLevelTower != null)
            _towerRange.text += " -> " + nextLevelTower.range;
        
        if(nextLevelTower != null)
            _upgradeButtonText.text = nextLevelTower.towerCost + " $";
        _sellButtonText.text = _selectedTower.GetSpentMoney().ToString() + " $";
    }

    private void UpdateUIPosition(){
        Vector3 offset = new Vector3(0, 2, 0);
        Vector3 uiPosition = _selectedTower.transform.position + offset;
        _visualsParent.transform.position = Camera.main.WorldToScreenPoint(uiPosition);
    }

    private void ClickedUpgradeButton(){
        SelectedTowerController.instance.UpgradeTower();
        UpdateVisuals();
    }

    private void ClickedDeleteButton(){
        SelectedTowerController.instance.SellTower();
        DisableVisuals();
    }

    private void EnableVisuals(){
        _visualsParent.SetActive(true);

        UpdateVisuals();
    }

    public void DisableVisuals(){
        _visualsParent.SetActive(false);
    }

    public void SetUpgradeButtonEnabled(bool toSet){
        _upgradeButton.interactable = toSet;
    }
    
    public void SetDeleteButtonEnabled(bool toSet){
        _sellButton.interactable = toSet;
    }

    private void UpdateVisuals(){
        SetUIElements();

        if(_selectedTower.GetIsUpgradeAvailable())
            _upgradeButton.gameObject.SetActive(true);
        else
            _upgradeButton.gameObject.SetActive(false);
    }

    public bool GetIsTowerSelected(){
        return _isTowerSelected;
    }

    public Vector3 GetUpgradeButtonPosition(){
        return _upgradeButton.transform.position;
    }

}
