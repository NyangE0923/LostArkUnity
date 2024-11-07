using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    // �� Ŭ���� ���� ���� ���¿� AttackCount�� Int���� ���� �ִϸ��̼��� ����ǵ��� �Ѵ�.
    // ���� �ʱ�ȭ �ð��� �Ǹ� ���� �ʱ�ȭ
    // 
    public override void EnterState(PlayerStateMachine.PLAYERSTATE state)
    {
        base.EnterState(state);

        info.IsAttacking = true;

        if (info.AttackCount > info.MaxAttackCount || Time.time >= info.LastAttackTime + info.AttackCountHoldTime )
        {
            info.AttackCount = 0;
        }

        info.Nav.updateRotation = false;
        info.Nav.velocity = Vector3.zero;
        info.Nav.isStopped = true;

        RotateTowardsMouseClick();

        info.Anim.SetInteger("State", (int)state);
        info.Anim.SetInteger("AttackCount", info.AttackCount);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // ���� ������� �ִϸ��̼��� 40% ~ 80% ��ŭ ������ �Ǿ����� ���콺 ��Ŭ���� �ߴٸ�
        // �޺� ������ Ȱ��ȭ�ϰ� �̾ ���� �� �� �ְ� �Ѵ�.
        AnimatorStateInfo animStateInfo = info.Anim.GetCurrentAnimatorStateInfo(0);
        if(animStateInfo.normalizedTime >= 0.4f &&  animStateInfo.normalizedTime <= 0.8f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                info.IsComboPossible = true;
            }
        }

        // ����Ƽ �̺�Ʈ Ʈ���Ÿ� ���� TriggerCalled������ true�� �Ǿ�����
        // isComboPossible�� true��� ���Ӱ�����, false��� Idle���·� �������´�.
        if (info.TriggerCalled)
        {
            info.Nav.ResetPath();

            if (info.IsComboPossible)
            {
                stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.ATTACK);
            }
            else
            {
                stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.IDLE);
            }
        }
    }

    public override void ExitState()
    {
        info.AttackCount++;
        info.Nav.updateRotation = true;
        info.Nav.isStopped = false;
        info.IsAttacking = false;
        info.LastAttackTime = Time.time;
        info.IsComboPossible = false;

        if(info.AttackEffect != null)
        {
            Destroy(info.AttackEffect, 0.7f);
        }
    }

    

    // �������� ������ �޼ҵ� (�������� ��ü�� ��� ������ �ִ� �ý��� �������� �������̽� ����)
    // �������� ���� �ִϸ��̼��� Ʈ���� �̺�Ʈ�� ���� ���������� Ȱ��ȭ ����
    // ���� �������̽��� �̿��� �������� �����ϴ� �̺�Ʈ �޼ҵ�
    public void TakeDamageTrigger()
    {
        Collider[] attackCheckColliders = Physics.OverlapSphere(info.AttackRange.position, info.AttackRadius, info.AttackLayer);

        foreach (var attackCheckCollider in attackCheckColliders)
        {
            if (attackCheckCollider != null)
            {
                EntityStats entityHealth = attackCheckCollider.GetComponent<EntityStats>();
                entityHealth.TakeDamage(info.NormalAttackdamage);

                EnemyHitEffect hitEffect = attackCheckCollider.GetComponentInChildren<EnemyHitEffect>();
                hitEffect.EnemyHit();
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (info != null && info.IsAttacking)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(info.AttackRange.position, info.AttackRadius);
        }
    }

    public void AttackEffectCreate()
    {
        Quaternion prefabRotation = info.AttackEffects[info.AttackCount].transform.rotation;
        Quaternion totalRotation = transform.rotation * prefabRotation;
        Vector3 prefabTransform = info.AttackEffects[info.AttackCount].transform.position;
        Vector3 totalTransform = transform.position + prefabTransform;


        info.AttackEffect = Instantiate(info.AttackEffects[info.AttackCount], totalTransform, totalRotation);
    }
}

