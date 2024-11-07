using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRendingTempest : ComboAttack
{
    [SerializeField] protected Transform centerPosition;

    public override void EnterState(int bossRandomSelect)
    {
        base.EnterState(bossRandomSelect);

        if (GroundManger.instance.CheckDestructionField())
        {
            nav.isStopped = false;
            nav.SetDestination(centerPosition.position);
        }
        else
        {
            nav.isStopped = true;
            nav.SetDestination(stateMachine.Player.transform.position);
        }

        info.Anim.SetInteger("State", 3);
        info.Anim.SetInteger("Attack", bossRandomSelect);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        AttackCombo(BossStateMachine.BOSSSTATE.IDLE);
    }
}
