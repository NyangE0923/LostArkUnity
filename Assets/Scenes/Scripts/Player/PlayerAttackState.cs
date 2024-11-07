using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    // 좌 클릭을 통해 현재 상태와 AttackCount의 Int값에 따라 애니메이션이 변경되도록 한다.
    // 공격 초기화 시간이 되면 스택 초기화
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

        // 현재 재생중인 애니메이션이 40% ~ 80% 만큼 진행이 되었을때 마우스 좌클릭을 했다면
        // 콤보 공격을 활성화하고 이어서 공격 할 수 있게 한다.
        AnimatorStateInfo animStateInfo = info.Anim.GetCurrentAnimatorStateInfo(0);
        if(animStateInfo.normalizedTime >= 0.4f &&  animStateInfo.normalizedTime <= 0.8f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                info.IsComboPossible = true;
            }
        }

        // 유니티 이벤트 트리거를 통해 TriggerCalled변수가 true가 되었을때
        // isComboPossible이 true라면 연속공격을, false라면 Idle상태로 빠져나온다.
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

    

    // 데미지를 전달할 메소드 (오버랩에 객체를 담고 데미지 주는 시스템 데미지는 인터페이스 구현)
    // 오버랩을 공격 애니메이션의 트리거 이벤트를 통해 순간적으로 활성화 시켜
    // 공격 인터페이스를 이용해 데미지를 전달하는 이벤트 메소드
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

