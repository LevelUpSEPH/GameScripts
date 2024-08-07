using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTutorial : TutorialBase
{
    private bool _anyBookCollected = false;

    protected override void Start(){
        base.Start();
        UpgradeItem.AnyUpgradeReceived += OnAnyUpgradeReceived;
    }

    protected void OnDisable(){
        UpgradeItem.AnyUpgradeReceived -= OnAnyUpgradeReceived;
    }

    private void OnAnyUpgradeReceived(){
        _anyBookCollected = true;
    }

    protected override bool HasMetTutorialCondition()
    {
        return _anyBookCollected;
    }
}
