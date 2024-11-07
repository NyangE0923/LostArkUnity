using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpecialAttackOne : BossSpecialAttack
{
    [SerializeField] private float chargingTimer;
    [SerializeField] private float chargingTime;

    public override void EnterState(BossStateMachine.BOSSSTATE state)
    {
        base.EnterState(state);
        nav.isStopped = false;
        nav.Warp(transform.position);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        BossMoveTranform(BossStateMachine.BOSSSTATE.BLOODSACRIFICE);
    }

    public void EndCharging(BossStateMachine.BOSSSTATE state)
    {
        if (chargingTimer >= chargingTime)
        {
            info.SpecialAttackAnimCount++;
            chargingTimer = 0;
            stateMachine.ChangeState(state);
        }
    }

    public void EndSpecialAttack()
    {
        info.SpecialAttackAnimCount = 0;
        // 특수 공격을 했는지 판단하고, Idle을 더욱 오래 지속하는 변수
        info.IsUseSpecialAttack = true;
        // 특수 공격을 하는 중을 나타내고, 변수가 true인 동안 Detect와 Idle의 시간계산을 멈추는 변수
        info.IsSpecialAttacking = false;
        nav.speed = info.NormalStateMoveSpeed;
        stateMachine.ChangeState(BossStateMachine.BOSSSTATE.IDLE);
    }

    protected virtual void BossMoveTranform(BossStateMachine.BOSSSTATE state)
    {

        if (info.SpecialAttackAnimCount == 0)
        {
            nav.speed = info.SpecialAttackMoveSpeed;
            nav.SetDestination(specialAttackPos.position);
            info.IsSpecialAttacking = true;

            if(!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance)
            {
                info.SpecialAttackAnimCount++;
                stateMachine.ChangeState(state);
                nav.Warp(specialAttackPos.position);
            }
        }

        //AnimatorStateInfo animStateInfo = info.Anim.GetCurrentAnimatorStateInfo(0);

        //// 애니메이션이 0.9 만큼 진행되었다면 다시 해당 상태로 진입된다.
        //if (animStateInfo.IsName("BossSpecialAttackOneStart") && animStateInfo.normalizedTime >= 0.9f)
        //{
        //    if (info.SpecialAttackAnimCount == 1)
        //    {
        //        info.SpecialAttackAnimCount++;
        //        stateMachine.ChangeState(state);
        //    }
        //}

        //if (animStateInfo.IsName("BossSpecialAttackOneCharging") && animStateInfo.normalizedTime >= 0.9f)
        //{
        //    chargingTimer += Time.deltaTime;

        //    if (chargingTimer >= chargingTime)
        //    {
        //        EndCharging(state);
        //    }
        //}

        //// 최대 특수 공격 카운트에 도달하면 공격하는 애니메이션이 재생되며
        //// 애니메이션이 재생되는 중에 이벤트 트리거로 지형이 파괴되는 이펙트와, 지형파괴를 델리게이트를 이용해 호출한다.
        //// 애니메이션이 0.9만큼 진행되었다면 아이들 상태로 돌아가고, 컷씬을 종료한다.
        //if (animStateInfo.IsName("BossSpecialOneAttack") && animStateInfo.normalizedTime >= 0.9f)
        //{
        //    if (info.SpecialAttackAnimCount == maxSpecialAttackAnimCount)
        //    {
        //        EndSpecialAttack();
        //    }
        //}

    }
}
