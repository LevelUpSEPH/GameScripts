using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TileBehaviour : MonoBehaviour
{
    // if is walkable, cant place turret. if placed turret, cant place another one. 
    // listen to event OnSwitchedGameState, have a state called placement mode, grid turns green if is placable

    [SerializeField] private Transform _towerPlacementTargetTransform;
    [SerializeField] private bool _isWalkable;
    
    private bool _isTowerPlacable;
    private TowerBase _towerOfTile;

    private void Start(){
        if(_isWalkable){
            _isTowerPlacable = false;
        }            
        else{
            _isTowerPlacable = true;
        }            

    }

    public void PlaceTower(TowerBase tower){
        
        tower.transform.position = _towerPlacementTargetTransform.position;
        _isTowerPlacable = false;
        _towerOfTile = tower;
    }

    public void RemoveTower(){
        if(_towerOfTile == null)
            return;
        
        _isTowerPlacable = true;
        _towerOfTile = null;
    }

    public TowerBase GetTileTower(){
        return _towerOfTile;
    }

    public bool GetIsTowerPlacable(){
        return _isTowerPlacable;
    }

    public Vector3 GetTowerPlacementPosition(){
        return _towerPlacementTargetTransform.position;
    }
}
