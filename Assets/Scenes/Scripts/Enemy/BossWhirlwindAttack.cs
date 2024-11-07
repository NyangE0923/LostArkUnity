using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWhirlwindAttack : BossNormalAttack
{
    [SerializeField] private float whirlWindAttackReadyTimer;
    [SerializeField] private float whirlWindAttackLoopTimer;
    [SerializeField] private float whirlWindAttackEndTimer;

    private bool isWhirlWindAttacking = false;

    public override void EnterState(int bossRandomSelect)
    {
        base.EnterState(bossRandomSelect);
        info.Anim.SetInteger("WhirlwindAttack", 0);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!isWhirlWindAttacking)
        {
            StartCoroutine(WhirWindAttackCoroutine());
        }
    }

    IEnumerator WhirWindAttackCoroutine()
    {
        isWhirlWindAttacking = true;

        yield return new WaitForSeconds(whirlWindAttackReadyTimer);
        info.Anim.SetInteger("WhirlwindAttack", 1);
        yield return new WaitForSeconds(whirlWindAttackLoopTimer);
        info.Anim.SetInteger("WhirlwindAttack", 2);
        yield return new WaitForSeconds(whirlWindAttackEndTimer);

        isWhirlWindAttacking = false;
        stateMachine.ChangeState(BossStateMachine.BOSSSTATE.IDLE);

    }
}
