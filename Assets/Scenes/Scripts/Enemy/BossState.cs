using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BossState : MonoBehaviour
{
    protected BossInfo info;
    protected BossStateMachine stateMachine;
    protected NavMeshAgent nav;
    protected BossStats stats;


    protected virtual void Awake()
    {
        stateMachine = GetComponent<BossStateMachine>();
        info = GetComponentInParent<BossInfo>();
        nav = GetComponentInParent<NavMeshAgent>();
        stats = GetComponentInParent<BossStats>();
    }

    public virtual void EnterState(BossStateMachine.BOSSSTATE state)
    {
        info.TriggerCalled = false;
    }

    public virtual void UpdateState()
    {
        if (info.IsDie)
        {
            return;
        }

        if(info.Stats.GroggyGauge <= 0 && !info.IsGroggy)
        {
            stateMachine.ChangeState(BossStateMachine.BOSSSTATE.GROGGY);
        }

        if (stats.Health <= info.DestructionGroundAttackHealth && !info.IsDestructionGroundAttack)
        {
            stateMachine.ChangeState(BossStateMachine.BOSSSTATE.DESTRUCTIONGROUND);
            info.IsDestructionGroundAttack = true;
        }

        if (stats.Health <= info.BloodSacrificeHealth && !info.IsBloodSacrificeAttack)
        {
            stateMachine.ChangeState(BossStateMachine.BOSSSTATE.BLOODSACRIFICE);
            info.IsBloodSacrificeAttack = true;
        }
    }

    public abstract void ExitState();

    public virtual void AnimationFinishTrigger()
    {
        info.TriggerCalled = true;
    }
    public void OnExitAttackState()
    {
        stateMachine.ChangeState(BossStateMachine.BOSSSTATE.IDLE);
    }
}
