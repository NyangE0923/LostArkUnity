using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlayerState : MonoBehaviour
{
    protected PlayerStateMachine stateMachine;
    protected PlayerInfo info;
    protected PlayerUseItem playerUseItem;
    protected PlayerStats stats;

    [SerializeField] protected LayerMask isGround;

    protected virtual void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
        info = GetComponent<PlayerInfo>();
        playerUseItem = GetComponent<PlayerUseItem>();
        stats = GetComponent<PlayerStats>();
    }

    // 상태전환 메소드, 플레이어의 상태머신에 있는 enum을 통해 상태전환
    // 상태가 전환될 때 TriggerCalled를 false로 바꾼다.
    public virtual void EnterState(PlayerStateMachine.PLAYERSTATE state)
    {
        info.TriggerCalled = false;
    }

    // 상태 업데이트 추상 메소드
    public virtual void UpdateState()
    {
        if(stats.Health <= 0)
        {
            stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.DIE);
        }

        if (info.IsDie || info.IsFallDown)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space) && !info.IsDashing)
        {
            Destroy(info.UseSkill.activeSkillEffect, 0.7f);
            StartCoroutine(Dash());
        }


        // 좌 클릭시 공격 상태로 전환
        if (Input.GetMouseButtonDown(0) && !info.IsAttacking && !info.IsSkill)
        {
            // 현재 사용하려는 아이템이 null이 아니고 사용할 준비가 되었다면 공격을 못하도록 한다.
            if (playerUseItem.usingItem != null && playerUseItem.usingItem.isItemAvailable)
            {
                return;
            }

            stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.ATTACK);
        }
    }

    public abstract void ExitState();

    IEnumerator Dash()
    {
        info.IsDashing = true;
        stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.DASH);
        yield return new WaitForSeconds(info.DashSkillCoolTime);
        info.IsDashing = false;
    }

    // 유니티 이벤트 트리거를 통한 애니메이션 전환을 위한 메소드
    // TriggerCalled를 true로 바꿈으로써 해당 애니메이션의 Update에서
    // 빠르게 상태 전환을 할 수 있다.
    public virtual void AnimationFinishTrigger()
    {
        info.TriggerCalled = true;
    }

    protected void RotateTowardsMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, isGround))
        {
            Vector3 targetDirection = hit.point - info.transform.position;

            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                info.transform.rotation = targetRotation;
            }
        }
    }

    public void OnAnimatorMove()
    {
        //
    }
}
