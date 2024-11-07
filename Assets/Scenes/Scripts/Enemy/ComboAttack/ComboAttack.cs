using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : BossNormalAttack
{
    [SerializeField] protected int maxAttackCount;
    [SerializeField] protected int bossRandomAttack;
    public override void EnterState(int bossRandomSelect)
    {
        base.EnterState(bossRandomSelect);
        bossRandomAttack = bossRandomSelect;
        info.Anim.SetInteger("AttackCount", attacksState.AttackCount);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    protected virtual void AttackCombo(BossStateMachine.BOSSSTATE state)
    {
        AnimatorStateInfo animStateInfo = info.Anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.normalizedTime >= 0.9f)
        {
            if (attacksState.AttackCount < maxAttackCount)
            {
                attacksState.AttackCount++;
                EnterState(bossRandomAttack);
            }
            else
            {
                stateMachine.ChangeState(state);
            }
        }
    }
}
