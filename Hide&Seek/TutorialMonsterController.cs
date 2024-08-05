using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMonsterController : MonsterController
{
    private bool _sawPlayer;

    protected override void OnSawPlayer()
    {
        base.OnSawPlayer();
        if(_sawPlayer)
            return;
        _sawPlayer = true;
        Catch(_playerController);
    }
}
