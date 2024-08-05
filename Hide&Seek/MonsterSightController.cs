using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSightController : MonoBehaviour
{
    [SerializeField] private MonsterLineOfSight _circularLOS;
    [SerializeField] private MonsterLineOfSight _forwardLOS;
    private MonsterControllerBase _monsterController;

    private void Start(){
        _monsterController = GetComponent<MonsterControllerBase>();
    }

    private void LateUpdate(){
        if(_forwardLOS != null && _forwardLOS.GetCanSeePlayer())
        {
            if(_forwardLOS.TryGetPlayerReference(out PlayerController playerController))
            {
                _monsterController.SetPlayerReference(playerController);
            }
            _monsterController.SetCanSeePlayer(true);
        }
        else if(_circularLOS != null && _circularLOS.GetCanSeePlayer())
        {
            if(_circularLOS.TryGetPlayerReference(out PlayerController playerController))
            {
                _monsterController.SetPlayerReference(playerController);
            }
            _monsterController.SetCanSeePlayer(true);
        }
        else
            _monsterController.SetCanSeePlayer(false);
    }
}
