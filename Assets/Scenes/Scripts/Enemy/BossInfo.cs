using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInfo : MonoBehaviour
{
    private Animator anim;
    private BossStats stats;
    [Header("SpecialPattern")]
    [SerializeField] private int bloodSacrificeHealth;
    [SerializeField] private bool isBloodSacrificeAttack;
    [SerializeField] private int destructionGroundAttackHealth;
    [SerializeField] private bool isDestructionGroundAttack;
    [Space]
    [Header("State")]
    [SerializeField] private bool isSpecialAttackDelay;
    [SerializeField] private bool isSpecialAttacking;
    [SerializeField] private bool isCounter;
    [SerializeField] private bool isGroggy;
    [SerializeField] private bool isDie;
    [Header("SpecialAttack")]
    [SerializeField] private int specialAttackAnimCount;
    [SerializeField] private float specialAttackMoveSpeed;
    [SerializeField] private float normalStateMoveSpeed;


    private bool triggerCalled;
    public bool TriggerCalled { get => triggerCalled; set => triggerCalled = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public BossStats Stats { get => stats; set => stats = value; }
    public bool IsCounter { get => isCounter; set => isCounter = value; }
    public bool IsGroggy { get => isGroggy; set => isGroggy = value; }
    public bool IsDie { get => isDie; set => isDie = value; }
    public int DestructionGroundAttackHealth { get => destructionGroundAttackHealth; set => destructionGroundAttackHealth = value; }
    public bool IsDestructionGroundAttack { get => isDestructionGroundAttack; set => isDestructionGroundAttack = value; }
    public bool IsUseSpecialAttack { get => isSpecialAttackDelay; set => isSpecialAttackDelay = value; }
    public int BloodSacrificeHealth { get => bloodSacrificeHealth; set => bloodSacrificeHealth = value; }
    public bool IsBloodSacrificeAttack { get => isBloodSacrificeAttack; set => isBloodSacrificeAttack = value; }
    public int SpecialAttackAnimCount { get => specialAttackAnimCount; set => specialAttackAnimCount = value; }
    public bool IsSpecialAttacking { get => isSpecialAttacking; set => isSpecialAttacking = value; }
    public float SpecialAttackMoveSpeed { get => specialAttackMoveSpeed; set => specialAttackMoveSpeed = value; }
    public float NormalStateMoveSpeed { get => normalStateMoveSpeed; set => normalStateMoveSpeed = value; }

    private void Awake()
    {
        Anim = GetComponentInParent<Animator>();
        Stats = GetComponentInParent<BossStats>();
    }
}
