using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossState
{
    [SerializeField] private float changeStateDelayTimer;
    [SerializeField] private float changeStateDelayTime;
    // 방금한 공격이 스페셜 공격이었다면 해당 변수를 더해줌
    [SerializeField] private float specialAttackDelayTime;

    public override void EnterState(BossStateMachine.BOSSSTATE state)
    {
        base.EnterState(state);
        changeStateDelayTimer = 0;
        if (info.IsUseSpecialAttack)
        {
            changeStateDelayTimer -= specialAttackDelayTime;
        }
        info.Anim.SetInteger("State", (int)state);
        nav.isStopped = true;
    }

    public override void ExitState()
    {
        info.TriggerCalled = true;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        changeStateDelayTimer += Time.deltaTime;

        if(changeStateDelayTimer >= changeStateDelayTime)
        {
            info.IsUseSpecialAttack = false;
            stateMachine.ChangeState(BossStateMachine.BOSSSTATE.DETECT);
        }
    }
}
