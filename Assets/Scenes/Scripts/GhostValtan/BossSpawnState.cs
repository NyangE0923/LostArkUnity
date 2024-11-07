using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnState : BossState
{
    [SerializeField] private BossUI bossUI;

    public override void EnterState(BossStateMachine.BOSSSTATE state)
    {
        base.EnterState(state);
        bossUI = FindAnyObjectByType<BossUI>();
        bossUI.InitGhostForm(info.Stats);
        info.Anim.SetInteger("State", (int)state);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();

        AnimatorStateInfo animStateInfo = info.Anim.GetCurrentAnimatorStateInfo(0);

        if (animStateInfo.normalizedTime >= 0.9f)
        {
            stateMachine.ChangeState(BossStateMachine.BOSSSTATE.IDLE);
        }
    }
}
