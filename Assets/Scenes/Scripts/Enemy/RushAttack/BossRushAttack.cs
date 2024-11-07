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
        // 현재 공격 카운트가 카운터 공격의 integer와 같다면
        if (attacksState.AttackCount == onCounter)
        {
            // 벽 파괴가 되어있다면 info.IsCounter를 true로 바꾸어준다.
            IsCounterState();

            // 카운터가 가능한 상태라면
            if (info.IsCounter)
            {
                // 카운터 이펙트를 활성화 한다.
                attacksState.CounterEffect.SetActive(true);
            }
        }
        else if (attacksState.AttackCount >= onCounter)
        {
            info.IsCounter = false;
            attacksState.CounterEffect.SetActive(false);
        }
    }

    // 카운터가 가능한 상태라면 info의 IsCounter를 true로 바꿔주는 메소드
    private void IsCounterState()
    {
        if (GroundManger.instance.CheckDestructionField())
        {
            info.IsCounter = true;
        }
    }
}
