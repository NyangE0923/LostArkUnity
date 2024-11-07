using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStateMachine;

public class IndicatorTrigger : MonoBehaviour
{
    [Header("Overlap Info")]
    [SerializeField] protected Collider[] playerColliders = new Collider[7];
    [SerializeField] protected LayerMask attackLayer;
    [Space]
    [Header("CrossAttack Info")]
    [SerializeField] protected Vector3 crossAttackVec;
    [Space]
    [Header("RendingAttack Info")]
    [SerializeField] protected float safeZoneRadius;
    [SerializeField] protected float dangerZoneRadius;
    [SerializeField] protected Collider[] safeZoneColliders = new Collider[7];
    [SerializeField] protected float rendingAttackIntensity;
    [SerializeField] protected float rendingAttackShakeTime;


    public void BossCrossAttackTrigger(int damage)
    {
        System.Array.Clear(playerColliders, 0, playerColliders.Length);
        int crossAttackColliders = Physics.OverlapBoxNonAlloc(transform.position, crossAttackVec / 2, playerColliders, transform.rotation, attackLayer);

        for (int i = 0; i < crossAttackColliders; i++)
        {
            Collider playerCollider = playerColliders[i];
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

    public void BossRendingTempestAttackTrigger(int damage)
    {
        System.Array.Clear(playerColliders, 0, playerColliders.Length);
        System.Array.Clear(safeZoneColliders, 0, safeZoneColliders.Length);
        int safeColliders = Physics.OverlapSphereNonAlloc(transform.position, safeZoneRadius, safeZoneColliders, attackLayer);
        int rendingTempestAttackColliders = Physics.OverlapSphereNonAlloc(transform.position, dangerZoneRadius, playerColliders, attackLayer);
        CameraManager.instance.ShakeCamera(rendingAttackIntensity, rendingAttackShakeTime);

        for (int i = 0; i < rendingTempestAttackColliders; i++)
        {
            Collider playerCollider = playerColliders[i];
            // ���� ������ �÷��̾ �ִ��� Ȯ��, �÷��̾ ���������� �ִٸ� true�� ��ȯ�Ͽ� �ش� �ݶ��̴��� �����ϰ� �Ѿ
            if(InSafeZone(playerCollider, safeZoneColliders, safeColliders))
            {
                continue;
            }

            PlayerStats playerHealth = playerCollider.GetComponent<PlayerStats>();
            PlayerStateMachine stateMachine = playerCollider.GetComponent<PlayerStateMachine>();
            PlayerInfo playerInfo = playerCollider.GetComponent<PlayerInfo>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            if (stateMachine != null)
            {
                if(stateMachine.CurrentState != stateMachine.PlayerStates[(int)PLAYERSTATE.FALLDOWN])
                {
                    stateMachine.ChangeState(PLAYERSTATE.FALLDOWN);
                }
            }
        }

        
    }

    // �÷��̾ ���ο� �ִ���, �ܺο� �ִ��� Ȯ���ϴ� �޼ҵ�
    // �÷��̾� �ݶ��̴�, ������ �� �迭, OverlapSphereNonAlloc���� ������ ��
    // ���������� ������ �� ��ŭ �ݺ������� �ش� �ݶ��̴��� �÷��̾��� �ݶ��̴����� Ȯ���ϰ�
    // �÷��̾��� �ݶ��̴���� true�� ��ȯ�ϰ� �� �ܿ��� false�� ��ȯ�ϴ� 'bool ��ȯ �޼ҵ�'
    private bool InSafeZone(Collider player, Collider[] inSafeZoneColliders, int safeColliders)
    {
        for (int i = 0; i < safeColliders; i++)
        {
            if (inSafeZoneColliders[i] == player)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, crossAttackVec);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, safeZoneRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dangerZoneRadius);
    }

}
