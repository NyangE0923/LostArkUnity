using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossTriggerManager : MonoBehaviour
{
    [SerializeField] protected Collider[] detectObjects = new Collider[15];

    [SerializeField] protected Transform bossAttackTransform;
    [SerializeField] protected float bossAttackRadius;
    [SerializeField] protected float bossWhirlWindAttackRadius;
    [SerializeField] protected LayerMask attackLayer;
    [SerializeField] protected LayerMask destroyableObject;
    [Header("Cross Attack")]
    [SerializeField] protected GameObject attackPos;
    [SerializeField] protected GameObject crossAttackPrefab;
    [SerializeField] protected GameObject createCrossAttack;
    [Header("Rending Attack")]
    [SerializeField] protected GameObject rendingAttackPrefab;
    [SerializeField] protected GameObject createRendingAttack;
    [SerializeField] protected float rendingAttackIntensity;
    [SerializeField] protected float rendingAttackShakeTime;
    [Header("Triple Attack")]
    [SerializeField] protected GameObject tripleAttackPrefab;
    [SerializeField] protected GameObject createTripleAttack;
    [SerializeField] protected GameObject tripleAttackEndPrefab;
    [SerializeField] protected GameObject createTripleAttackEnd;
    [SerializeField] protected float normalAttackIntensity;
    [SerializeField] protected float normalAttackShakeTime;
    [Header("WhirlWind Attack")]
    [SerializeField] protected GameObject whirlWindAttackPrefab;
    [SerializeField] protected GameObject createWhirlWindAttack;
    [SerializeField] protected Transform whirlWindTransform;
    [SerializeField] protected float whirlWindAttackIntensity;
    [SerializeField] protected float whirlWindAttackShakeTime;
    [Header("Destruction Field")]
    [SerializeField] protected BossSpecialAttack bossSpecialAttack;
    [SerializeField] protected GameObject destructionFieldEffectPrefab;
    [SerializeField] protected GameObject createDestructionFieldEffect;
    [SerializeField] protected Transform destructionFieldTransform;
    [Header("SpecialAttackCount")]
    [SerializeField] protected BossInfo bossInfo;
    [SerializeField] protected BossSpecialAttackOne bossSpecialAttackOne;
    [Header("Ready")]
    [SerializeField] protected GameObject specialAttackReadyEffectPrefab;
    [SerializeField] protected GameObject createSpecialAttackReadyEffect;
    [Space]
    [Header("Attack")]
    [SerializeField] protected GameObject specialAttackEffectPrefab;
    [SerializeField] protected GameObject createSpecialAttackEffect;
    [Space]
    [Header("Indicator")]
    [SerializeField] protected GameObject specialAttackIndicatorPrefab;
    [SerializeField] protected GameObject createSpecialAttackIndicator;

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawWireSphere(whirlWindTransform.position, bossWhirlWindAttackRadius);
    //}
    private void Start()
    {
        bossInfo = GetComponentInChildren<BossInfo>();
        bossSpecialAttack = GetComponentInChildren<BossSpecialAttack>();
    }


    #region BossSpecialAttackOne

    public void BossSpecialAttackCount()
    {
        bossSpecialAttack.BossSpecialAttackMove(BossStateMachine.BOSSSTATE.BLOODSACRIFICE);
    }

    public void EndSpecialAttack()
    {
        bossSpecialAttackOne.EndSpecialAttack();
    }

    public void EndCharging()
    {
        bossSpecialAttackOne.EndCharging(BossStateMachine.BOSSSTATE.BLOODSACRIFICE);
    }
    #region Effect
    public void BossSpecialAttackCount(int count)
    {
        bossInfo.SpecialAttackAnimCount += count;
    }

    // 공격 준비 이펙트 트리거 메소드
    public void CreateBossSpecialAttackReadyEffect()
    {

    }

    // 공격(휘두름) 이펙트 트리거 메소드
    public void CreateBossSpecialAttackEffect()
    {

    }

    // 공격 범위 이펙트 트리거 메소드
    public void CreateBossSpecialAttackIndicator()
    {

    }
    #endregion
    #endregion

    #region SpecialAttackThree
    public void BossSpecialAttackMove()
    {
        bossSpecialAttack.BossSpecialAttackMove(BossStateMachine.BOSSSTATE.DESTRUCTIONGROUND);
    }

    public void BossSpecialAttackZoomIn()
    {
        bossSpecialAttack.BossSpecialAttackZoomIn(BossStateMachine.BOSSSTATE.DESTRUCTIONGROUND);
    }

    public void BossSpecialAttackZoomOut()
    {
        bossSpecialAttack.BossSpecialAttackZoomOut(BossStateMachine.BOSSSTATE.DESTRUCTIONGROUND);
    }

    public void BossSpecialAttackEnd()
    {
        bossSpecialAttack.BossSpecialAttackEnd(BossStateMachine.BOSSSTATE.DESTRUCTIONGROUND);
    }
    #endregion

    public void BossNormalAttackTrigger(int damage)
    {
        System.Array.Clear(detectObjects, 0, detectObjects.Length);
        int detectPlayerColliders = Physics.OverlapSphereNonAlloc(bossAttackTransform.position, bossAttackRadius, detectObjects, attackLayer);
        CameraManager.instance.ShakeCamera(normalAttackIntensity, normalAttackShakeTime);

        for (int i = 0; i < detectPlayerColliders; i++)
        {
            Collider playerCollider = detectObjects[i];
            PlayerStats playerHealth = playerCollider.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            else
            {
                continue;
            }
        }
    }

    public void BossWhirlWindAttackTrigger(int damage)
    {
        System.Array.Clear(detectObjects, 0, detectObjects.Length);
        int detectPlayerColliders = Physics.OverlapSphereNonAlloc(transform.position, bossWhirlWindAttackRadius, detectObjects, attackLayer | destroyableObject);

        for (int i = 0; i < detectPlayerColliders; i++)
        {
            Collider collider = detectObjects[i];
            PlayerStats playerHealth = collider.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            ObjectStats objectHealth = collider.GetComponent<ObjectStats>();
            EnemyHitEffect enemyHitEffect = collider.GetComponent<EnemyHitEffect>();
            if (objectHealth != null)
            {
                objectHealth.TakeDamage(damage);
                enemyHitEffect.EnemyHit();
            }

        }
    }

    public void CreateTripleSmashEffect()
    {
        Quaternion prefabRotation = tripleAttackPrefab.transform.rotation;
        Quaternion totalRotation = attackPos.transform.rotation * prefabRotation;

        createTripleAttack = Instantiate(tripleAttackPrefab, attackPos.transform.position, totalRotation);
    }

    public void CreateTripleSmashEndEffect()
    {
        Quaternion prefabRotation = tripleAttackEndPrefab.transform.rotation;
        Quaternion totalRotation = attackPos.transform.rotation * prefabRotation;

        createTripleAttackEnd = Instantiate(tripleAttackEndPrefab, attackPos.transform.position, totalRotation);
    }

    public void CreateCrossAttackRange()
    {
        createCrossAttack = Instantiate(crossAttackPrefab, attackPos.transform.position, Quaternion.identity);
    }

    public void CreateRendingAttackRange()
    {
        CameraManager.instance.ShakeCamera(rendingAttackIntensity, rendingAttackShakeTime);
        createRendingAttack = Instantiate(rendingAttackPrefab, attackPos.transform.position, Quaternion.identity);
    }

    public void CreateWhirlWindAttackEffect()
    {
        CameraManager.instance.ShakeCamera(whirlWindAttackIntensity, whirlWindAttackShakeTime);
        createWhirlWindAttack = Instantiate(whirlWindAttackPrefab, whirlWindTransform.position, whirlWindTransform.rotation);
    }

    public void CreateDestructionFieldEffect()
    {
        Quaternion prefabRotation = destructionFieldEffectPrefab.transform.rotation;
        Quaternion totalRotation = destructionFieldTransform.rotation * prefabRotation;
        createDestructionFieldEffect = Instantiate(destructionFieldEffectPrefab, destructionFieldTransform.position, totalRotation);
    }
}
