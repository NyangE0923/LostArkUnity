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
        // Ư�� ������ �ߴ��� �Ǵ��ϰ�, Idle�� ���� ���� �����ϴ� ����
        info.IsUseSpecialAttack = true;
        // Ư�� ������ �ϴ� ���� ��Ÿ����, ������ true�� ���� Detect�� Idle�� �ð������ ���ߴ� ����
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

        //// �ִϸ��̼��� 0.9 ��ŭ ����Ǿ��ٸ� �ٽ� �ش� ���·� ���Եȴ�.
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

        //// �ִ� Ư�� ���� ī��Ʈ�� �����ϸ� �����ϴ� �ִϸ��̼��� ����Ǹ�
        //// �ִϸ��̼��� ����Ǵ� �߿� �̺�Ʈ Ʈ���ŷ� ������ �ı��Ǵ� ����Ʈ��, �����ı��� ��������Ʈ�� �̿��� ȣ���Ѵ�.
        //// �ִϸ��̼��� 0.9��ŭ ����Ǿ��ٸ� ���̵� ���·� ���ư���, �ƾ��� �����Ѵ�.
        //if (animStateInfo.IsName("BossSpecialOneAttack") && animStateInfo.normalizedTime >= 0.9f)
        //{
        //    if (info.SpecialAttackAnimCount == maxSpecialAttackAnimCount)
        //    {
        //        EndSpecialAttack();
        //    }
        //}

    }
}
