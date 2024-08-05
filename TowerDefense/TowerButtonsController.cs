using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButtonsController : Singleton<TowerButtonsController>
{
    [SerializeField] private List<TowerButton> _towerButtons = new List<TowerButton>();

    private void Start(){
        InitializeList();
    }

    private void InitializeList(){
        foreach(Transform child in transform){
            if(!child.gameObject.activeInHierarchy)
                continue;
            TowerButton towerButton = child.gameObject.GetComponent<TowerButton>();
            _towerButtons.Add(towerButton);
        }
    }

    public void SetTowerButtonsEnabled(bool toSet){
        foreach(TowerButton towerButton in _towerButtons)
            towerButton.SetTowerButtonActive(toSet);
    }

    public List<TowerButton> GetTowerButtons(){
        return _towerButtons;
    }

    public void DeactivateButtons(){
        foreach(TowerButton towerButton in _towerButtons)
            towerButton.gameObject.SetActive(false);
    }

    public void ActivateButtons(){
        foreach(TowerButton towerButton in _towerButtons)
            towerButton.gameObject.SetActive(true);
    }
}
