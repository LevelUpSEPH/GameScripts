using System.Collections.Generic;
using UnityEngine;

public class PlayerSightController : MonoBehaviour // might cause performance issues/ might not either? 
{
    [SerializeField] private PlayerLineOfSight _playerSightLong;
    [SerializeField] private PlayerLineOfSight _playerSightCircular;

    private float _catchAmountPerTick = 20f;
    private PlayerController _playerController;

    private List<TargetControllerBase> _targets = new List<TargetControllerBase>();

    private void OnEnable(){
        InLevelController.GameStarted += OnGameStarted;
        UpgradeController.SightUpgraded += OnSightUpgraded;

        SetSightAngles();
        _playerController = GetComponent<PlayerController>();
    }

    private void OnDisable()
    {
        InLevelController.GameStarted -= OnGameStarted;
        UpgradeController.SightUpgraded -= OnSightUpgraded;
    }

    private void OnSightUpgraded()
    {
        SetSightAngles();
    }

    private void SetSightAngles()
    {
        Debug.Log(UpgradeController.instance.sight[PlayerData.Instance.SightLevel - 1].coef);
        _playerSightLong.SetAngleMultiplier(UpgradeController.instance.sight[PlayerData.Instance.SightLevel - 1].coef);
        _playerSightCircular.SetAngle(360 - _playerSightLong.GetAngle());
    }

    private void OnGameStarted()
    {
        _catchAmountPerTick = UpgradeController.instance.sightDamage[PlayerData.Instance.SightDamageLevel - 1].value;
    }

    void LateUpdate()
    {
        List<TargetControllerBase> _newTargets = new List<TargetControllerBase>();
        if(!_playerController.GetIsPlayerHiding())
        {

                foreach(TargetControllerBase target in _playerSightLong.GetTargetControllers())
                {
                    if(_newTargets.Contains(target))
                        continue;
                    _newTargets.Add(target);
                    target.ShowTarget(_catchAmountPerTick);
                }
                foreach(TargetControllerBase target in _playerSightCircular.GetTargetControllers())
                {
                    if(_newTargets.Contains(target))
                        continue;
                    _newTargets.Add(target);
                    target.ShowTarget(_catchAmountPerTick);
                }
        }

        foreach(TargetControllerBase targetController in _targets){
            if(!_newTargets.Contains(targetController)){
                targetController.HideTarget();
            }
        }

        _targets = new List<TargetControllerBase>(_newTargets);

        foreach(TargetControllerBase targetController in _targets){
            if(targetController.GetCaughtPercentage() >= 1f){
                _playerController.Catch(targetController.GetComponent<IHider>());
            }
        }
    }

}
