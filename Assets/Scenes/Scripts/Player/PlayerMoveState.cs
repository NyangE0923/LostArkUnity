using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public override void EnterState(PlayerStateMachine.PLAYERSTATE state)
    {
        base.EnterState(state);
        // 애니메이션 재생
        info.Anim.SetInteger("State", (int)state);
        // 이동
        Movement();
    }

    private void Movement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, isGround))
        {
            info.Nav.SetDestination(hit.point);
        }
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (Input.GetMouseButton(1))
        {
            Movement();
        }

        if (info.Nav.remainingDistance < 0.1f)
        {
            info.Nav.Warp(transform.position);
            stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.IDLE);
        }
        // 목표 위치가 거의 비슷해졌다면 Idle상태로 전환
    }
}
