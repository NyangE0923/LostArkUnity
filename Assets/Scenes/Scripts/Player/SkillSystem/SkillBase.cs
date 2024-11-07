using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "CreateSkill")]
public class SkillBase : ScriptableObject
{
    [Header("#SKILL INFO")]
    public int skillDamage;
    public float skillRadius;
    public Vector3 skillVector;
    public float skillCoolTime;
    public float skillCoolTimer;
    public LayerMask AttackLayer;
    public string skillAnimationTrigger;
    public int manaCost;
    public int shockDamage;
    [Space]
    [Header("#SKILL DATA")]
    public Sprite Icon;
    public GameObject[] skillEffects;
    [Space]
    [Header("#COMBO SKILL")]
    public int comboCount;
    public int maxComboCount = 1;
    public bool isComboSkill = false;
    public bool useComboSkill = false;
    //public float comboCountHoldTime; 필요시를 대비해 만들어둔 변수
    [Space]
    [Header("#COUNTER SKILL")]
    public bool isCounterSkill = false;
    [Space]
    [Header("#ESTHER SKILL")]
    public bool isEstherSkill = false;
    //Silian 실리안

    public void SkillCoolTimeInitialize()
    {
        skillCoolTimer = skillCoolTime;
    }

    // 애니메이션 작동 메소드
    public virtual void AplySkill(PlayerUseSkill useSkill)
    {
        useSkill.info.Anim.SetTrigger(skillAnimationTrigger);
        useSkill.info.Anim.SetInteger("ComboCount", comboCount);
    }

    // 스킬의 데미지 메소드
    public void TakeSkillDamageTrigger(Transform skillPosition)
    {
        Collider[] skillCheckColliders = Physics.OverlapSphere(skillPosition.position, skillRadius, AttackLayer);

        foreach (Collider skillCheckCollider in skillCheckColliders)
        {
            if (skillCheckCollider != null)
            {
                EntityStats entityHealth = skillCheckCollider.GetComponent<EntityStats>();
                entityHealth.TakeDamage(skillDamage);

                BossStats bossStats = skillCheckCollider.GetComponent<BossStats>();
                bossStats.TakeShockDamage(shockDamage);

                EnemyHitEffect hitEffect = skillCheckCollider.GetComponentInChildren<EnemyHitEffect>();
                hitEffect.EnemyHit();

                if (isCounterSkill)
                {
                    BossInfo bossinfo = skillCheckCollider.GetComponentInChildren<BossInfo>();
                    BossStateMachine stateMachine = skillCheckCollider.GetComponentInChildren<BossStateMachine>();

                    if(bossinfo != null && bossinfo.IsCounter)
                    {
                        stateMachine.ChangeState(BossStateMachine.BOSSSTATE.GROGGY);

                    }
                }
            }
        }
    }

    // 에스더 스킬 데미지 메소드 Invoke를 이용하여 호출
    public void TakeEstherSkillDamage(Transform estherSkillPosition)
    {
        Collider[] estherSkillCheckColliders = Physics.OverlapBox(estherSkillPosition.position, skillVector / 2, estherSkillPosition.rotation, AttackLayer);

        foreach (Collider estherSkillCheckCollider in estherSkillCheckColliders)
        {
            if (estherSkillCheckCollider != null)
            {
                EntityStats entityHealth = estherSkillCheckCollider.GetComponent<EntityStats>();
                entityHealth.TakeDamage(skillDamage);

                BossStats bossStats = estherSkillCheckCollider.GetComponent<BossStats>();
                bossStats.DestructionArmor();

                EnemyHitEffect hitEffect = estherSkillCheckCollider.GetComponentInChildren<EnemyHitEffect>();
                hitEffect.EnemyHit();
            }
        }
    }

    // 스킬의 이펙트 생성 메소드
    public GameObject SkillEffectCreate(Transform playerPos)
    {
        Quaternion prefabRotation = skillEffects[comboCount].transform.rotation;
        Quaternion totalRotation = playerPos.rotation * prefabRotation;

        GameObject skillInstance = Instantiate(skillEffects[comboCount], playerPos.position, totalRotation);
        return skillInstance;
    }
}
