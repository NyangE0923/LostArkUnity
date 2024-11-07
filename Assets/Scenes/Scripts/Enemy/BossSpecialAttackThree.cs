using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BossSpecialAttackThree : BossSpecialAttack
{
    [SerializeField] private bool isDestructionDelegate;

    public override void EnterState(BossStateMachine.BOSSSTATE state)
    {
        base.EnterState(state);
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (info.SpecialAttackAnimCount == maxSpecialAttackAnimCount)
        {
            if (!isDestructionDelegate)
            {
                GroundManger.onDestructionGround();
                Debug.Log("델리게이트 호출");
                isDestructionDelegate = true;
            }
        }
    }
}
