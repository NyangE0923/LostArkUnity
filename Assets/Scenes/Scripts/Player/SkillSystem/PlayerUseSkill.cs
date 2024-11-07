using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 방안 : 스크립터블 오브젝트로 스킬을 구현하고
// ScriptableObject(BaseSkill) -> 스킬
// PlayerUseSkill -> 스킬들을 배열에 담아놓고 쿨타임 계산, 사용 가능여부 및 사용여부 판단
// SkillController -> UIManager, PlayerStateMachine, PlayerUseSkill를 모두 가지고 있으며 StateMachine을 통해 Idle상태로 나갈 수 있도록 만들어줌 

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

    #region 스킬 쿨타임 계산 및 사용가능 여부
    // 스킬 쿨타임 계산 후 true or false를 반환하는 메소드
    public bool CanUseSkill_CoolTime(SKILL skillIndex)
    {
        return playerSkills[(int)skillIndex].skillCoolTimer >= playerSkills[(int)skillIndex].skillCoolTime;
    }
    public bool CanUseSkill_ManaPoint(SKILL skillIndex)
    {
        return playerStats.ManaPoint >= playerSkills[(int)skillIndex].manaCost;
    }
    // 스킬 사용 메소드
    public void UseSkill(SKILL skillIndex)
    {
        usingSkill = playerSkills[(int)skillIndex];
        usingSkill.AplySkill(this);
        usingSkill.skillCoolTimer = 0;
        playerStats.ManaPoint -= usingSkill.manaCost;
    }
    #endregion

    // 사용하려는 스킬의 데미지 메소드를 사용하기 위한 이벤트 트리거 메소드
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

    // Idle로 전환하는 메소드
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
            // 사용중인 스킬이 베스티지라면 애니메이션의 deltaPos값을 3배로 준다.
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
