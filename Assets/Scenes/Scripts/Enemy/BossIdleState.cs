using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossState
{
    [SerializeField] private float changeStateDelayTimer;
    [SerializeField] private float changeStateDelayTime;
    // ����� ������ ����� �����̾��ٸ� �ش� ������ ������
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
