using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerFallDownState : PlayerState
{
    [SerializeField] private int fallDownAnimationNum;
    [SerializeField] private int maxFallDownAnimationNum;
    [SerializeField] private Vector3 knockbackDir;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackTimer;
    private Rigidbody rb;

    public override void EnterState(PlayerStateMachine.PLAYERSTATE state)
    {
        rb = GetComponent<Rigidbody>();

        base.EnterState(state);
        if (!info.IsFallDown)
        {
            knockbackDir = (transform.position - info.CurrentBoss.transform.position).normalized;
            knockbackTimer = knockbackTime;

            info.IsFallDown = true;
            fallDownAnimationNum = 0;
            if (GroundManger.instance.CheckDestructionField())
            {
                info.Nav.isStopped = true;
                info.Nav.enabled = false;
            }
            else
            {
                info.Nav.isStopped = true;
            }
        }
        info.Anim.SetInteger("State", (int)state);
        info.Anim.SetInteger("FallDown", fallDownAnimationNum);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        knockbackTimer -= Time.deltaTime;

        if(knockbackTimer >= 0)
        {
            transform.Translate(knockbackDir * knockbackSpeed * Time.deltaTime, Space.World);
        }

        AnimatorStateInfo animStateInfo = info.Anim.GetCurrentAnimatorStateInfo(0);

        if ((fallDownAnimationNum == 0 && animStateInfo.IsName("FallDown01") && animStateInfo.normalizedTime >= 0.9f) ||
            (fallDownAnimationNum == 1 && animStateInfo.IsName("FallDown02") && animStateInfo.normalizedTime >= 0.9f) ||
            (fallDownAnimationNum == 2 && animStateInfo.IsName("FallDown03") && animStateInfo.normalizedTime >= 0.9f))
        {
            // ���� ������ �ı��� ���¶��
            if (GroundManger.instance.CheckDestructionField())
            {
                // �÷��̾ �׺� �Ž��� ������ ����ٸ�
                if (!IsOnNavMesh())
                {
                    // �׺� �Ž��� ��Ȱ��ȭ��Ű�� �ִϸ��̼� �ѹ��� 0���� �����ѵ�
                    // ���ٿ� ������ false�� �ٲٰ�, Rigidbody�� Kinematic ������ ��Ȱ��ȭ ����
                    // Rigidbody�� ���� �浹�� Ȱ��ȭ ��Ų��.
                    // ü���� 0���� �����ѵ� Die���·� �Ѿ���� �Ѵ�.
                    info.Nav.enabled = false;
                    fallDownAnimationNum = 0;
                    info.IsFallDown = false;
                    rb.isKinematic = false;
                    stats.Health = 0;
                    stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.DIE);
                }
            }
            else
            {
                // �����ı��� ���� ���� ���¶��
                if (fallDownAnimationNum < maxFallDownAnimationNum)
                {
                    fallDownAnimationNum++;
                    stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.FALLDOWN);
                }
                else
                {
                    if (!info.Nav.enabled)
                    {
                        info.Nav.enabled = true;
                    }
                    fallDownAnimationNum = 0;
                    info.Nav.isStopped = false;
                    info.IsFallDown = false;
                    info.Nav.Warp(transform.position);
                    stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.IDLE);
                }
            }
        }
    }

    private bool IsOnNavMesh()
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(transform.position, out hit, 0.1f, NavMesh.AllAreas);
    }
}
