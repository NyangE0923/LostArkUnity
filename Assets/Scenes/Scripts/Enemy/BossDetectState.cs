using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDetectState : BossState
{
    [SerializeField] protected float attackTimeDelay;
    [SerializeField] protected float maxAttackTimeDelay;
    [SerializeField] protected float idleTimeDelay;
    [SerializeField] protected float maxIdleTimeDelay;
    [SerializeField] protected bool isAttacking;
    [SerializeField] protected bool isIdle;
    [SerializeField] protected float attackDistance;

    [SerializeField] protected LayerMask player;


    public override void EnterState(BossStateMachine.BOSSSTATE state)
    {
        base.EnterState(state);
        nav.isStopped = false;
        info.Anim.SetInteger("State", (int)state);
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (info.IsSpecialAttacking) return;

        nav.SetDestination(stateMachine.Player.transform.position);

        idleTimeDelay += Time.deltaTime;
        attackTimeDelay += Time.deltaTime;

        float currentDistanceToPlayer = Vector3.Distance(transform.position, stateMachine.Player.transform.position);

        if(idleTimeDelay >= maxIdleTimeDelay)
        {
            idleTimeDelay = 0;
            stateMachine.ChangeState(BossStateMachine.BOSSSTATE.IDLE);
        }
        if(attackTimeDelay >= maxAttackTimeDelay && currentDistanceToPlayer <= attackDistance + 0.1f)
        {
            attackTimeDelay = 0;
            stateMachine.ChangeState(BossStateMachine.BOSSSTATE.ATTACK);
        }
    }
}
