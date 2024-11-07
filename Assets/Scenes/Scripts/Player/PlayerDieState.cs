using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    public override void EnterState(PlayerStateMachine.PLAYERSTATE state)
    {
        base.EnterState(state);
        info.Anim.SetInteger("State", (int)state);
        if (info.Nav.enabled)
        {
            info.Nav.isStopped = true;
            info.Nav.Warp(transform.position);
        }
        info.IsDie = true;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
