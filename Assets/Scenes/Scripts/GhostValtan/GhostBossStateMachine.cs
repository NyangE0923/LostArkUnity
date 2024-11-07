using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBossStateMachine : BossStateMachine
{
    protected override void Start()
    {
        Player = GameObject.FindWithTag("Player");
        ChangeState(BOSSSTATE.SPAWN);
    }
}
