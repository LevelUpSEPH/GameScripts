using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private GameObject _towerToPlace;
    [SerializeField] private TextMeshProUGUI _towerCost;
    private TowerBase _towerBase;
    private Button _towerButton;

    private bool _canPlace = false;

    private void Awake(){
        _towerBase = _towerToPlace.GetComponent<TowerBase>();
        _towerCost.text = _towerBase.GetTowerCost().ToString() + '$';
    }

    private void Start(){
        _towerButton = GetComponent<Button>();
    }

    private void OnEnable(){
        MoneyController.MoneyAmountChanged += UpdateTowerButtons;
        TowerBase.TowerCountChanged += OnTowerCountChanged;
        LevelBehaviour.Started += UpdateTowerButtons;
    }

    private void OnDisable(){
        MoneyController.MoneyAmountChanged -= UpdateTowerButtons;
        TowerBase.TowerCountChanged -= OnTowerCountChanged;
        LevelBehaviour.Started -= UpdateTowerButtons;
    }

    public void ShowTowerButton(){
        _towerButton.gameObject.SetActive(true);
    }
    
    public void OnPointerDown(){
        if(!CanAffordTower() || !_towerButton.interactable){
            return;
        }

        StopAllCoroutines();
        _canPlace = false;
        StartCoroutine(PlacementCountdown());
            
        PlacementController.instance.SetTowerToPlace(_towerToPlace.GetComponent<TowerBase>());
        
        GameStateManager.instance.SwitchGameStateToPlacement();
    }

    private void UpdateTowerButtons(){
        if(TutorialController.instance.GetIsTutorialActive())
            return;
        if(CanPlaceTower())
            _towerButton.interactable = true;
        else
            _towerButton.interactable = false;
        //Debug.Log(_towerButton.interactable);
    }

    private void OnTowerCountChanged(bool unused){
        UpdateTowerButtons();
    }

    private bool CanPlaceTower(){
        if(MoneyController.instance.GetMoney() >= _towerBase.GetTowerCost() && InLevelController.instance.GetLessThanMaxTowerPlaced())
            return true;
        else
            return false;
    }

    private bool CanAffordTower(){
        if(MoneyController.instance.GetHasEnoughMoneyFor(_towerBase.GetTowerCost())){
            return true;
        }
        return false;
    }

    public void SetTowerButtonActive(bool toSet){
        _towerButton.interactable = toSet;
        if(!CanPlaceTower())
            _towerButton.interactable = false;
    }

    public TowerSO.TowerType GetTowerType(){
        return _towerBase.GetTowerType();
    }

    public void OnPointerUp(){
        if(_canPlace)
            PlacementController.instance.OnPointerUp();
        else
            PlacementController.instance.CancelPlacement();
    }

    public void OnPointerEnter(){
        if(GameStateManager.instance.GetGameState() == GameStateManager.GameState.PlayingState)
            return;
        if(PlacementController.instance.GetTowerToPlace().GetTowerType() == _towerBase.GetTowerType())
            PlacementController.instance.CancelPlacement();
    }

    private IEnumerator PlacementCountdown(){
        yield return new WaitForSeconds(0.1f);
        _canPlace = true;
    }

}
