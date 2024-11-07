using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// ��� : ��ũ���ͺ� ������Ʈ�� ��ų�� �����ϰ�
// ScriptableObject(BaseSkill) -> ��ų
// PlayerUseSkill -> ��ų���� �迭�� ��Ƴ��� ��Ÿ�� ���, ��� ���ɿ��� �� ��뿩�� �Ǵ�
// SkillController -> UIManager, PlayerStateMachine, PlayerUseSkill�� ��� ������ ������ StateMachine�� ���� Idle���·� ���� �� �ֵ��� ������� 

public class PlayerUseSkill : MonoBehaviour
{
    public enum SKILL { SOULSINUS, VESTIGE, LUNATICEDGE, RUSTNAIL, SILIAN }
    public SkillBase usingSkill;
    public SkillBase[] playerSkills;
    public PlayerInfo info;
    public PlayerStateMachine stateMachine;
    public GameObject activeSkillEffect;
    public PlayerUseEstherSkill estherSkill;

    [SerializeField] private Transform skillPosition;
    [SerializeField] private PlayerStats playerStats;

    private void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
        info = GetComponent<PlayerInfo>();
        playerStats = GetComponent<PlayerStats>();
        estherSkill = GetComponent<PlayerUseEstherSkill>();

        for (int i = 0; i < playerSkills.Length; i++)
        {
            playerSkills[i].SkillCoolTimeInitialize();
        }
    }



    private void Update()
    {
        for (int i = 0; i < playerSkills.Length; i++)
        {
            if (playerSkills[i].skillCoolTimer <= playerSkills[i].skillCoolTime)
            {
                playerSkills[i].skillCoolTimer += Time.deltaTime;
            }
        }

        if (info.TriggerCalled)
        {
            transform.position = info.Anim.rootPosition;
            info.Nav.Warp(transform.position);
            ExitSkillState();
        }
    }

    #region ��ų ��Ÿ�� ��� �� ��밡�� ����
    // ��ų ��Ÿ�� ��� �� true or false�� ��ȯ�ϴ� �޼ҵ�
    public bool CanUseSkill_CoolTime(SKILL skillIndex)
    {
        return playerSkills[(int)skillIndex].skillCoolTimer >= playerSkills[(int)skillIndex].skillCoolTime;
    }
    public bool CanUseSkill_ManaPoint(SKILL skillIndex)
    {
        return playerStats.ManaPoint >= playerSkills[(int)skillIndex].manaCost;
    }
    // ��ų ��� �޼ҵ�
    public void UseSkill(SKILL skillIndex)
    {
        usingSkill = playerSkills[(int)skillIndex];
        usingSkill.AplySkill(this);
        usingSkill.skillCoolTimer = 0;
        playerStats.ManaPoint -= usingSkill.manaCost;
    }
    #endregion

    // ����Ϸ��� ��ų�� ������ �޼ҵ带 ����ϱ� ���� �̺�Ʈ Ʈ���� �޼ҵ�
    public void OnSkillHit()
    {
        if(usingSkill != null)
        {
            usingSkill.TakeSkillDamageTrigger(skillPosition);
        }
    }

    public void OnSkillEffect()
    {
        if(usingSkill != null)
        {
            activeSkillEffect = usingSkill.SkillEffectCreate(transform);
        }
    }

    public void CreateEstherSkillEffect()
    {
        if (usingSkill != null)
        {
            activeSkillEffect = usingSkill.SkillEffectCreate(estherSkill.estherSkillPosition.transform);
        }

    }
    protected void OnDrawGizmos()
    {
        if (usingSkill != null && usingSkill.isEstherSkill && estherSkill.estherSkillPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(estherSkill.estherSkillPosition.transform.position, usingSkill.skillVector / 2);
        }

        if (info.IsSkill)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(skillPosition.position, usingSkill.skillRadius);
        }
    }

    // Idle�� ��ȯ�ϴ� �޼ҵ�
    public void ExitSkillState()
    {
        info.IsSkill = false;
        Destroy(activeSkillEffect, 0.7f);
        stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.IDLE);
    }
    public void OnAnimatorMove()
    {
        if (info.IsSkill && usingSkill != null)
        {
            // ������� ��ų�� ����Ƽ����� �ִϸ��̼��� deltaPos���� 3��� �ش�.
            if(usingSkill == playerSkills[(int)SKILL.VESTIGE])
            {
                Vector3 deltaPos = info.Anim.deltaPosition;
                deltaPos *= 2.5f;
                transform.position += deltaPos;
            }
            else
            {
                Vector3 deltaPos = info.Anim.deltaPosition;
                transform.position += deltaPos;
            }
        }

    }

}
