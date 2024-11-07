using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossNormalAttack : BossState
{
    // 이동 금지
    // 각 공격 모션 작동
    // 인디케이터 생성 or 일반 공격인데 일반 공격을 베이스로 구현하고, 인디케이터는 상속받아서 구현하자.
    // 버츄얼 공격 메소드 생성

    protected BossAttacksState attacksState;

    protected override void Awake()
    {
        base.Awake();
        attacksState = GetComponent<BossAttacksState>();
    }

    public virtual void EnterState(int bossRandomSelect)
    {
        info.TriggerCalled = false;
        nav.isStopped = true;
        info.Anim.SetInteger("State", 3);
        info.Anim.SetInteger("Attack", bossRandomSelect);
    }

    public override void ExitState()
    { 

    }

    public override void UpdateState()
    {
        base.UpdateState();

    }
}
