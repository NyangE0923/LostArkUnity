using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacksState : BossState
{
    // , SMASHANDBURST, CHARGEATTACK
    public enum BOSSATTACK { TRIPLESMASH, WHIRLWIND, CROSSATTACK, RENDINGTEMPEST, RUSHATTACK}
    [SerializeField] protected BossNormalAttack[] bossAttacks;
    [SerializeField] protected BossNormalAttack selectBossAttack;
    [SerializeField] protected int bossRandomSelect;
    [SerializeField] private int attackCount;
    [SerializeField] private GameObject counterEffect;

    public int AttackCount { get => attackCount; set => attackCount = value; }
    public GameObject CounterEffect { get => counterEffect; set => counterEffect = value; }

    public override void EnterState(BossStateMachine.BOSSSTATE state)
    {
        AttackCount = 0;
        base.EnterState(state);
        bossRandomSelect = Random.Range(0, bossAttacks.Length);
        SelectAttack();
    }

    public override void ExitState()
    {
        info.IsCounter = false;
        counterEffect.SetActive(false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        selectBossAttack?.UpdateState();
    }

    public void SelectAttack()
    {
        selectBossAttack?.ExitState();
        selectBossAttack = bossAttacks[bossRandomSelect];
        selectBossAttack.EnterState(bossRandomSelect);
    }

    public void ChangeAttack(BOSSATTACK attack)
    {
        selectBossAttack?.ExitState();
        selectBossAttack = bossAttacks[(int)attack];
        selectBossAttack.EnterState((int)attack);
    }
}
