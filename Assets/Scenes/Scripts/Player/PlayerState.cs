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

    // ������ȯ �޼ҵ�, �÷��̾��� ���¸ӽſ� �ִ� enum�� ���� ������ȯ
    // ���°� ��ȯ�� �� TriggerCalled�� false�� �ٲ۴�.
    public virtual void EnterState(PlayerStateMachine.PLAYERSTATE state)
    {
        info.TriggerCalled = false;
    }

    // ���� ������Ʈ �߻� �޼ҵ�
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


        // �� Ŭ���� ���� ���·� ��ȯ
        if (Input.GetMouseButtonDown(0) && !info.IsAttacking && !info.IsSkill)
        {
            // ���� ����Ϸ��� �������� null�� �ƴϰ� ����� �غ� �Ǿ��ٸ� ������ ���ϵ��� �Ѵ�.
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

    // ����Ƽ �̺�Ʈ Ʈ���Ÿ� ���� �ִϸ��̼� ��ȯ�� ���� �޼ҵ�
    // TriggerCalled�� true�� �ٲ����ν� �ش� �ִϸ��̼��� Update����
    // ������ ���� ��ȯ�� �� �� �ִ�.
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
