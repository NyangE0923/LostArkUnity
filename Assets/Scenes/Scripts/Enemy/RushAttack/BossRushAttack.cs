using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRushAttack : ComboAttack
{
    [SerializeField] private int onCounter;

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

        OnCounterEffect();
        AttackCombo(BossStateMachine.BOSSSTATE.IDLE);
    }

    private void OnCounterEffect()
    {
        // ���� ���� ī��Ʈ�� ī���� ������ integer�� ���ٸ�
        if (attacksState.AttackCount == onCounter)
        {
            // �� �ı��� �Ǿ��ִٸ� info.IsCounter�� true�� �ٲپ��ش�.
            IsCounterState();

            // ī���Ͱ� ������ ���¶��
            if (info.IsCounter)
            {
                // ī���� ����Ʈ�� Ȱ��ȭ �Ѵ�.
                attacksState.CounterEffect.SetActive(true);
            }
        }
        else if (attacksState.AttackCount >= onCounter)
        {
            info.IsCounter = false;
            attacksState.CounterEffect.SetActive(false);
        }
    }

    // ī���Ͱ� ������ ���¶�� info�� IsCounter�� true�� �ٲ��ִ� �޼ҵ�
    private void IsCounterState()
    {
        if (GroundManger.instance.CheckDestructionField())
        {
            info.IsCounter = true;
        }
    }
}
