using System.Collections.Generic;
using UnityEngine;

public class PlayerLineOfSight : LineOfSight
{
    private List<TargetControllerBase> _targetsInSight = new List<TargetControllerBase>();
    private float _meshResolutionBase;

    protected override void Start()
    {
        base.Start();
        _meshResolutionBase = _meshResolution;

        PlayerController.PlayerHidingStateChanged += OnHidingStateChanged;
        InLevelController.GameStarted += OnGameStarted;
    }

    private void OnDestroy()
    {
        PlayerController.PlayerHidingStateChanged -= OnHidingStateChanged;
        InLevelController.GameStarted -= OnGameStarted;
    }

    public void SetAngleMultiplier(float angleMultiplier)
    {
        _angleMultiplier = angleMultiplier;
    }

    public void SetAngle(float angle)
    {
        _angle = angle;
    }

    protected override void FieldofViewCheck(){
        _targetsInSight.Clear();
        if(_isGameActive)
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetLayer);
            if(rangeChecks.Length > 0){
                for(int i = 0; i < rangeChecks.Length; i++){
                    Transform target = rangeChecks[i].transform;
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if(Vector3.Angle(transform.forward, directionToTarget) < (_angle * _angleMultiplier)/2) {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);
                        if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionLayer)){
                            TargetControllerBase targetController = target.GetComponent<TargetControllerBase>();
                            _targetsInSight.Add(targetController);
                        }
                    }
                }
            }
        }
    }

    private void OnHidingStateChanged(bool isHiding)
    {
        if(isHiding)
        {
            _meshResolution = 0;
        }
        else
        {
            _meshResolution = _meshResolutionBase;
        }
    }

    private void OnGameStarted()
    {
        _radius = _radius * UpgradeController.instance.sight[PlayerData.Instance.SightLevel - 1].coef;
    }

    public List<TargetControllerBase> GetTargetControllers(){
        return _targetsInSight;
    }
}
