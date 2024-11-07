using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    [SerializeField] private float manaPoint;
    [SerializeField] private float maxManaPoint;
    [SerializeField] private float manaRegen;
    [SerializeField] private float manaRegenRate;
    public float ManaPoint { get => manaPoint; set => manaPoint = value; }
    public float MaxManaPoint { get => maxManaPoint; set => maxManaPoint = value; }
    public float ManaRegen { get => manaRegen; set => manaRegen = value; }
    [SerializeField] private PlayerStateMachine stateMachine;

    private bool useManaRegenerateCoroutine = false;

    private void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    protected override void Start()
    {
        base.Start();
        ManaPoint = MaxManaPoint;
    }

    protected override void Update()
    {
        base.Update();

        if(ManaPoint < MaxManaPoint && !useManaRegenerateCoroutine)
        {
            useManaRegenerateCoroutine = true;
            StartCoroutine(manaRegenerate());
        }
    }

    IEnumerator manaRegenerate()
    {
        while (ManaPoint < MaxManaPoint)
        {
            yield return new WaitForSeconds(manaRegenRate);
            ManaPoint += ManaRegen;
            ManaPoint = Mathf.Clamp(ManaPoint, 0, MaxManaPoint);
        }
        useManaRegenerateCoroutine = false;
    }

    protected override void Die()
    {
        base.Die();
        stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.DIE);
    }

    // 현재 마나 값 업데이트
    // BaseSkill에서 각 스킬마다 필요마나 수치 설정
    // 
}
