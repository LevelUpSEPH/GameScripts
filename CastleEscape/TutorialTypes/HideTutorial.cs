using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTutorial : TutorialBase
{
    private bool _playerHid = false;

    protected override void Start()
    {
        base.Start();
        BarrelHide.PlayerHid += OnPlayerHid;
    }

    private void OnDisable(){
        BarrelHide.PlayerHid -= OnPlayerHid;
    }

    private void OnPlayerHid(){
        _playerHid = true;
    }

    protected override bool HasMetTutorialCondition()
    {
        return _playerHid;
    }


}
