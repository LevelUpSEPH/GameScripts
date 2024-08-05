using UnityEngine;

public class MonsterLineOfSight : LineOfSight
{   
    private bool _canSeePlayer = false;
    private PlayerController _playerController;

    protected override void FieldofViewCheck(){
        if(_isGameActive)
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetLayer);
            if(rangeChecks.Length > 0){
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if(Vector3.Angle(transform.forward, directionToTarget) < _angle/2) {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionLayer)){
                        _playerController = target.GetComponent<PlayerController>();
                        if(!_playerController.GetIsPlayerHiding() || _playerController.GetIsMoving()){
                            _canSeePlayer = true;
                            return;
                        }
                    }
                }
            }
        }
        _canSeePlayer = false;
    }

    public bool GetCanSeePlayer(){
        return _canSeePlayer;
    }

    public bool TryGetPlayerReference(out PlayerController playerController)
    {
        if(_playerController != null)
        {
            playerController = _playerController;
            return true;
        }
        else
        {
            playerController = null;
            return false;
        }
    }
}
