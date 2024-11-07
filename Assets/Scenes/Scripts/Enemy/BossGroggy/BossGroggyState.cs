using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroggyState : BossState
{
    public override void EnterState(BossStateMachine.BOSSSTATE state)
    {
        base.EnterState(state);
        nav.isStopped = true;
        info.Anim.SetInteger("State", (int)state);
        info.IsGroggy = true;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        AnimatorStateInfo animStateInfo = info.Anim.GetCurrentAnimatorStateInfo(0);

        if (animStateInfo.IsName("BossGroggy") && animStateInfo.normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(BossStateMachine.BOSSSTATE.STUN);
        }
    }
}
