using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : EntityStats
{
    // 스택당 받는 피해 감소 10%
    // 발탄이 그로기 상태라면
    // 총 스택 3개
    [SerializeField] private int armorCount;
    [SerializeField] private int maxArmorCount;
    [SerializeField] private int groggyGauge;
    [SerializeField] private int maxGroggyGauge;
    [SerializeField] private float groggyGaugeInitTimer;
    [SerializeField] private float groggyGaugeInitTime;

    private BossInfo bossInfo;
    private BossStateMachine stateMachine;

    public int ArmorCount { get => armorCount; set => armorCount = value; }
    public int MaxArmorCount { get => maxArmorCount; set => maxArmorCount = value; }
    public int GroggyGauge { get => groggyGauge; set => groggyGauge = value; }
    public int MaxGroggyGauge { get => maxGroggyGauge; set => maxGroggyGauge = value; }
    public BossStateMachine StateMachine { get => stateMachine; set => stateMachine = value; }

    public void Awake()
    {
        ArmorCount = MaxArmorCount;
        GroggyGauge = MaxGroggyGauge;
        bossInfo = GetComponentInChildren<BossInfo>();
        stateMachine = GetComponentInChildren<BossStateMachine>();
    }

    public override void TakeDamage(int damage)
    {
        float totalDamage =  damage - (damage * ArmorCount / 10);

        if (bossInfo.IsSpecialAttacking)
        {
            totalDamage = 0;
        }

        Health -= (int)totalDamage;
        Health = Mathf.Clamp(Health, MinHealth, MaxHealth);

        if (Health <= MinHealth)
        {
            Health = MinHealth;
            Die();
        }
    }

    public void TakeShockDamage(int shockDamage)
    {
        if (!bossInfo.IsDie)
        {
            GroggyGauge -= shockDamage;
            GroggyGauge = Mathf.Clamp(GroggyGauge, 0, int.MaxValue);
        }
    }

    public void GroggyGaugeInitialize()
    {
        GroggyGauge = MaxGroggyGauge;
    }

    protected override void Die()
    {
        base.Die();
        stateMachine.ChangeState(BossStateMachine.BOSSSTATE.DIE);
    }

    public void DestructionArmor()
    {
        if (bossInfo.IsGroggy)
        {
            ArmorCount--;
            ArmorCount = Mathf.Clamp(ArmorCount, 0, int.MaxValue);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (GroggyGaugeZero())
        {
            groggyGaugeInitTimer += Time.deltaTime;

            if (groggyGaugeInitTimer >= groggyGaugeInitTime)
            {
                GroggyGaugeInitialize();
                groggyGaugeInitTimer = 0f;
            }
        }
    }

    private bool GroggyGaugeZero()
    {
        return GroggyGauge <= 0;
    }
}
