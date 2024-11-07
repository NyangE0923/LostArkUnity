using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrossAttack : BossNormalAttack
{
    public override void EnterState(int bossRandomSelect)
    {
        base.EnterState(bossRandomSelect);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        AnimatorStateInfo animStateInfo = info.Anim.GetCurrentAnimatorStateInfo(0);

        if (animStateInfo.normalizedTime >= 1f)
        {
            attacksState.ChangeAttack(BossAttacksState.BOSSATTACK.WHIRLWIND);
        }

    }
}
