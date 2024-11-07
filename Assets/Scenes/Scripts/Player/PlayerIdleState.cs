using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public override void EnterState(PlayerStateMachine.PLAYERSTATE state)
    {
        base.EnterState(state);
        info.Nav.Warp(transform.position);
        info.Anim.SetInteger("State", (int)state);
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (info.IsDie || info.IsFallDown)
        {
            return;
        }

        base.UpdateState();
        // Ŭ���� �̵� ���·� ��ȯ
        if (Input.GetMouseButton(1))
        {
            stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.MOVE);
        }
    }
}
