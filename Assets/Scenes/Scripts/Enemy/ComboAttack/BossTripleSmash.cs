using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTripleSmash : ComboAttack
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
        AttackCombo(BossStateMachine.BOSSSTATE.DETECT);
    }
}
