using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyTutorial : TutorialBase
{
    private bool _anyUnitDied = false;

    protected override void Start()
    {
        base.Start();
        Unit.AnyUnitDied += OnAnyUnitDied;
    }

    private void OnDisable(){
        Unit.AnyUnitDied -= OnAnyUnitDied;
    }

    private void OnAnyUnitDied(Unit unused){
        _anyUnitDied = true;
    }

    protected override bool HasMetTutorialCondition()
    {
        return _anyUnitDied;
    }

}
