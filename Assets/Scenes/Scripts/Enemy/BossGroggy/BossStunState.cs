using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStunState : BossState
{
    [SerializeField] private float stunDurationTimer;
    [SerializeField] private float stunDurationTime;

    public override void EnterState(BossStateMachine.BOSSSTATE state)
    {
        base.EnterState(state);
        info.Anim.SetInteger("State", (int)state);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();

        stunDurationTimer += Time.deltaTime;
        if(stunDurationTimer >= stunDurationTime)
        {
            info.IsGroggy = false;
            stunDurationTimer = 0f;
            stateMachine.ChangeState(BossStateMachine.BOSSSTATE.IDLE);
        }
    }
}
