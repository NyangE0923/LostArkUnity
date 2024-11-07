using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossNormalAttack : BossState
{
    // �̵� ����
    // �� ���� ��� �۵�
    // �ε������� ���� or �Ϲ� �����ε� �Ϲ� ������ ���̽��� �����ϰ�, �ε������ʹ� ��ӹ޾Ƽ� ��������.
    // ����� ���� �޼ҵ� ����

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
