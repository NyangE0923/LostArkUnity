using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerState
{
    public override void EnterState(PlayerStateMachine.PLAYERSTATE state)
    {
        base.EnterState(state);
        info.Nav.Warp(transform.position);
        info.Anim.SetInteger("State", (int)state);

        RotateTowardsMouseClick();
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
