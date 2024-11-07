using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInfo : MonoBehaviour
{
    protected Animator anim;
    protected NavMeshAgent nav;
    protected bool triggerCalled;

    [Header("Move info")]
    [SerializeField] protected float defaultSpeed;
    [SerializeField] protected float moveSpeed = 8f;

    [Header("Dash info")]
    [SerializeField] protected float dashSpeed = 50f;
    [SerializeField] protected float dashSkillCooltime = 7f;
    [SerializeField] protected float dashDistance = 10f;
    [SerializeField] protected bool isDash = false;
    [SerializeField] protected float detectionRadius = 0.5f;
    [SerializeField] protected GameObject dashEffect;

    [Header("Attack info")]
    [SerializeField] protected int attackCount = 0;
    [SerializeField] protected int maxAttackCount = 1;
    [SerializeField] protected float resetAttackTimer = 1f;
    [SerializeField] protected float attackTimer;
    [SerializeField] protected float lastAttackTime;
    [SerializeField] protected float attackCountHoldTime = 1f;
    [SerializeField] protected bool isAttacking;
    [SerializeField] protected bool isComboPossible;
    [Space]
    [SerializeField] protected Transform attackRange;
    [SerializeField] protected float attackRadius = 5;
    [SerializeField] protected LayerMask attackLayer;
    [SerializeField] protected int normalAttackdamage = 10;
    [SerializeField] protected GameObject[] attackEffects;
    [SerializeField] protected GameObject attackEfffect;
    [Space]
    [Header("Skill info")]
    [SerializeField] protected bool isSkill = false;
    [SerializeField] private PlayerUseSkill useSkill;
    [Space]
    [Header("Die info")]
    [SerializeField] private bool isDie = false;
    [Space]
    [Header("FallDown info")]
    [SerializeField] private bool isFallDown = false;
    [SerializeField] private GameObject currentBoss;

    public Animator Anim { get => anim; set => anim = value; }
    public NavMeshAgent Nav { get => nav; set => nav = value; }
    public float DefaultSpeed { get => defaultSpeed; set => defaultSpeed = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float DashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float DashSkillCoolTime { get => dashSkillCooltime; set => dashSkillCooltime = value; }
    public float DashDistance { get => dashDistance; set => dashDistance = value; }
    public bool IsDashing { get => isDash; set => isDash = value; }
    public int AttackCount { get => attackCount; set => attackCount = value; }
    public float ResetAttackTimer { get => resetAttackTimer; set => resetAttackTimer = value; }
    public float AttackTimer { get => attackTimer; set => attackTimer = value; }
    public int MaxAttackCount { get => maxAttackCount; set => maxAttackCount = value; }
    public bool TriggerCalled { get => triggerCalled; set => triggerCalled = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    //무한 재귀를 방지하기 위해 isComboPossible 과 value가 같지 않다면 set을 하도록 수정
    public bool IsComboPossible
    {
        get
        {
            return isComboPossible;
        }
        set
        {
            if(isComboPossible != value)
            {
                isComboPossible = value;
            }
        }
    }
    public float LastAttackTime { get => lastAttackTime; set => lastAttackTime = value; }
    public float AttackCountHoldTime { get => attackCountHoldTime; set => attackCountHoldTime = value; }
    public float DetectionRadius { get => detectionRadius; set => detectionRadius = value; }
    public Transform AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackRadius { get => attackRadius; set => attackRadius = value; }
    public LayerMask AttackLayer { get => attackLayer; set => attackLayer = value; }
    public int NormalAttackdamage { get => normalAttackdamage; set => normalAttackdamage = value; }
    public bool IsSkill { get => isSkill; set => isSkill = value; }
    public PlayerUseSkill UseSkill { get => useSkill; set => useSkill = value; }
    public GameObject[] AttackEffects { get => attackEffects; set => attackEffects = value; }
    public GameObject AttackEffect { get => attackEfffect; set => attackEfffect = value; }
    public GameObject DashEffect { get => dashEffect; set => dashEffect = value; }
    public bool IsDie { get => isDie; set => isDie = value; }
    public bool IsFallDown { get => isFallDown; set => isFallDown = value; }
    public GameObject CurrentBoss { get => currentBoss; set => currentBoss = value; }

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        defaultSpeed = moveSpeed;
        nav.speed = defaultSpeed;
        UseSkill = GetComponent<PlayerUseSkill>();
    }

    private void Start()
    {
        UpdateCurrentBoss("Valtan");
    }

    private void UpdateCurrentBoss(string bossObjectName)
    {
        if(CurrentBoss == null)
        {
            CurrentBoss = GameObject.Find(bossObjectName);
        }
    }

    private void Update()
    {
        UpdateCurrentBoss("GhostValtan(Clone)");
    }
}
