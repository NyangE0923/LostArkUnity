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
            // 벽과 지형이 파괴된 상태라면
            if (GroundManger.instance.CheckDestructionField())
            {
                // 플레이어가 네브 매쉬의 영역을 벗어났다면
                if (!IsOnNavMesh())
                {
                    // 네브 매쉬를 비활성화시키고 애니메이션 넘버를 0으로 변경한뒤
                    // 폴다운 변수를 false로 바꾸고, Rigidbody의 Kinematic 변수를 비활성화 시켜
                    // Rigidbody에 의한 충돌을 활성화 시킨다.
                    // 체력을 0으로 변경한뒤 Die상태로 넘어가도록 한다.
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
                // 지형파괴가 되지 않은 상태라면
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
