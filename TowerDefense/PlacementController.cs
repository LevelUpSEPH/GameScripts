using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlacementController : Singleton<PlacementController>
{
    public static event Action PlacedTower;
    private TowerBase _towerToPlace;

    [SerializeField] private TowerBase _singleTargetTower;
    [SerializeField] private TowerBase _aoeTower;
    [SerializeField] private TowerBase _circleAoeTower;
    [SerializeField] private TowerBase _sniperTower;

    private TileBehaviour _mouseOverTile = null;

    private void Start(){
        GameStateManager.GameStateChanged += OnGameStateChanged;
        //InputController.PointerUp += OnPointerUp;
    }

    private void OnDisable(){
        GameStateManager.GameStateChanged -= OnGameStateChanged;
        //InputController.PointerUp -= OnPointerUp;
    }

    private void Update(){
        if(GameStateManager.instance.GetGameState() == GameStateManager.GameState.PlacementState){
            RaycastHit raycastHit = MouseController.instance.GetMouseWorldPositionHit();
            if(raycastHit.collider == null)
                return;
            if(raycastHit.collider.TryGetComponent<TileBehaviour>(out TileBehaviour tile)){
                if(_mouseOverTile != tile){
                _mouseOverTile = tile;
                OnMouseTileChanged();
            }
                _towerToPlace.transform.position = tile.GetTowerPlacementPosition() + Vector3.up;

            }
        }
         
    }

    public void CancelPlacement(){
        if(GameStateManager.instance.GetGameState() == GameStateManager.GameState.PlayingState)
            return;
        Debug.Log("Cancelled placement");
        _towerToPlace.gameObject.SetActive(false);
        GameStateManager.instance.SwitchGameStateToPlaying();
    }

    private bool TryPlace(TileBehaviour tileBehaviour){
        if(!PlayerData.Instance.TowersTutorialFinished){
            if(tileBehaviour != ActiveTutorialController.instance.GetTutorialTile())
                return false;
        }

        if(MoneyController.instance.TryUseMoney(_towerToPlace.GetTowerCost())){
            GameObject tower = Instantiate(_towerToPlace.gameObject);
            TowerBase towerBase = tower.GetComponent<TowerBase>();
            _towerToPlace.gameObject.SetActive(false);

            towerBase.Placed(tileBehaviour);
            tileBehaviour.PlaceTower(towerBase);

            _towerToPlace = null;
            PlacedTower?.Invoke();
            return true;
        }

        return false;

    }

    private void OnMouseTileChanged(){

        if(_mouseOverTile.GetIsTowerPlacable())
            _towerToPlace.SetPlacementVisualsPlacibility(true);
        else
            _towerToPlace.SetPlacementVisualsPlacibility(false);
    }

    private void OnGameStateChanged(GameStateManager.GameState gameState){
        if(gameState != GameStateManager.GameState.PlacementState)
            return;

        _towerToPlace.gameObject.SetActive(true);
    }

    public void SetTowerToPlace(TowerBase tower){
        if(tower == _singleTargetTower)
            _towerToPlace = _singleTargetTower;
        else if(tower == _sniperTower)
            _towerToPlace = _sniperTower;
        else if(tower == _aoeTower)
            _towerToPlace = _aoeTower;
        else
            _towerToPlace = _circleAoeTower;
    }

    public TowerBase GetTowerToPlace(){
        return _towerToPlace;
    }

    public void OnPointerUp(){
        // will not work for iphone, use IphoneOnPointerUp(Input.mousePosition);
        OnPointerUp(Input.mousePosition);
    }

    private void OnPointerUp(Vector3 mousePos){
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        if(GameStateManager.instance.GetGameState() == GameStateManager.GameState.PlacementState){

            RaycastHit raycastHit = MouseController.instance.GetWorldPositionFromScreen(mousePos2d);

            if(raycastHit.collider == null){
                CancelPlacement();
                GameStateManager.instance.SwitchGameStateToPlaying();
                return;
            }
                

            if(raycastHit.collider.TryGetComponent<TileBehaviour>(out TileBehaviour tileBehaviour)){
                if(!tileBehaviour.GetIsTowerPlacable()) 
                    CancelPlacement();
                else
                    if(!TryPlace(tileBehaviour))
                        CancelPlacement();
            }
            else
                CancelPlacement();

            GameStateManager.instance.SwitchGameStateToPlaying();

        }
    }
    
}
