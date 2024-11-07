using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpecialAttack : BossState
{
    [SerializeField] protected Transform specialAttackPos;
    [SerializeField] protected int maxSpecialAttackAnimCount;

    public override void EnterState(BossStateMachine.BOSSSTATE state)
    {
        base.EnterState(state);
        info.Anim.SetInteger("State", (int)state);
        info.Anim.SetInteger("SpecialAttackAnimCount", info.SpecialAttackAnimCount);
        info.IsSpecialAttacking = true;


    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();

        
    }
    public void BossSpecialAttackMove(BossStateMachine.BOSSSTATE state)
    {
        info.SpecialAttackAnimCount++;
        stateMachine.ChangeState(state);
    }
    public void BossSpecialAttackZoomIn(BossStateMachine.BOSSSTATE state)
    {
        nav.Warp(specialAttackPos.position);
        CameraManager.instance.BossCutscene();
    }
    public void BossSpecialAttackZoomOut(BossStateMachine.BOSSSTATE state)
    {
        CameraManager.instance.EndBossCutScene();
        info.IsUseSpecialAttack = true;
    }
    public void BossSpecialAttackEnd(BossStateMachine.BOSSSTATE state)
    {
        info.SpecialAttackAnimCount = 0;
        info.IsSpecialAttacking = false;
        Debug.Log("스페셜 공격 끝");
        stateMachine.ChangeState(BossStateMachine.BOSSSTATE.IDLE);
    }
}
