using System;

public class SelectedTowerController : Singleton<SelectedTowerController>
{
    public static event Action SelectedTower;
    public static event Action DeselectedTower;

    private TowerBase _selectedTower;
    private bool _isTowerSelected = false;

    private void Start(){
        PlayerMovementController.MovementStateChanged += OnPlayerMoved;
    }

    private void OnDisable(){
        PlayerMovementController.MovementStateChanged -= OnPlayerMoved;
    }

    public void SelectTower(TowerBase tower){
        _selectedTower = tower;

        _selectedTower.DisplayRange();
        SelectedTowerControllerUI.instance.DisplaySelectedTowerInfo(_selectedTower);
        _isTowerSelected = true;
        SelectedTower?.Invoke();
    }

    public void DeselectTower(){
        if(!_isTowerSelected)
            return;
        _selectedTower.HideRange();
        _selectedTower = null;
        SelectedTowerControllerUI.instance.DisableVisuals();
        _isTowerSelected = false;
        DeselectedTower?.Invoke();
    }

    public void SellTower(){
        _selectedTower.SellTower();
    }

    public void UpgradeTower(){
        _selectedTower.TryUpgradeTower();
    }

    public TowerBase GetSelectedTower(){
        return _selectedTower;
    }

    private void OnPlayerMoved(bool isMoving){
        if(isMoving)
            DeselectTower();
    }

}
