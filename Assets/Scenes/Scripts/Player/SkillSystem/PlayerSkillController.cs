using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSkillController : MonoBehaviour
{
    public Animator anim;
    public PlayerUseSkill useSkill;
    public PlayerUI playerUI;
    public PlayerUseEstherSkill estherSkill;
    public PlayerInfo info;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        useSkill = GetComponent<PlayerUseSkill>();
        estherSkill = GetComponent<PlayerUseEstherSkill>();
        playerUI = FindObjectOfType<PlayerUI>();
        info = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        playerUI.UpdateSkillUI(useSkill.playerSkills);

        if (info.IsDie || info.IsFallDown)
        {
            return;
        }

        if (useSkill.usingSkill != null)
        {
            TryUseComboSkill(KeyCode.E, PlayerUseSkill.SKILL.LUNATICEDGE);
        }

        if (useSkill.info.IsSkill) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryUseSkill(PlayerUseSkill.SKILL.SOULSINUS);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            TryUseSkill(PlayerUseSkill.SKILL.VESTIGE);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryUseSkill(PlayerUseSkill.SKILL.LUNATICEDGE);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryUseSkill(PlayerUseSkill.SKILL.RUSTNAIL);
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.G))
        {
            TryUseEstherSkill(PlayerUseSkill.SKILL.SILIAN);
        }
    }

    private void TryUseSkill(PlayerUseSkill.SKILL skillName)
    {
        if (useSkill.CanUseSkill_CoolTime(skillName) && useSkill.CanUseSkill_ManaPoint(skillName))
        {
            if(useSkill.usingSkill != null)
            {
                ComboSkillCountReset();
            }

            useSkill.stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.SKILL);
            useSkill.info.IsSkill = true;
            useSkill.UseSkill(skillName);

            if(useSkill.usingSkill != null)
            {
                if (useSkill.usingSkill.isComboSkill)
                {
                    useSkill.usingSkill.useComboSkill = true;

                    if (useSkill.usingSkill.isComboSkill && useSkill.usingSkill.comboCount < useSkill.usingSkill.maxComboCount)
                    {
                        useSkill.usingSkill.comboCount++;
                    }
                }
            }
        }
    }
    private void TryUseComboSkill(KeyCode keyCode, PlayerUseSkill.SKILL skillName)
    {
        AnimatorStateInfo animStateInfo = useSkill.info.Anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.normalizedTime >= 0.6f && animStateInfo.normalizedTime <= 0.9f)
        {

            if (Input.GetKey(keyCode))
            {
                if(useSkill.usingSkill.useComboSkill)
                {
                    Destroy(useSkill.activeSkillEffect, 0.7f);
                    useSkill.stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.SKILL);
                    useSkill.info.IsSkill = true;
                    useSkill.UseSkill(skillName);
                    useSkill.usingSkill.useComboSkill = false;

                    ComboSkillCountReset();
                }
            }
        }
    }
    private void TryUseEstherSkill(PlayerUseSkill.SKILL skillName)
    {
        // 에스더 게이지가 모두 회복 되었다면
        if (estherSkill.EstherGaugeCheck())
        {
            // 에스더 스킬을 사용하기 위한 Position 프리팹 생성
            estherSkill.EstherSkillPositionCreate();
            useSkill.usingSkill = useSkill.playerSkills[(int)skillName];
            useSkill.CreateEstherSkillEffect();
            Invoke("TakeEstherSkillDamage", 0.4f);
            //useSkill.usingSkill.TakeEstherSkillDamage(estherSkill.estherSkillPosition.transform);
            estherSkill.estherGauge = 0;
            Destroy(useSkill.activeSkillEffect, 4f);
            Destroy(estherSkill.estherSkillPosition, 5f);
        }
    }

    private void ComboSkillCountReset()
    {
        if (useSkill.usingSkill.comboCount == useSkill.usingSkill.maxComboCount)
        {
            useSkill.usingSkill.comboCount = 0;
        }
    }
    public void TakeEstherSkillDamage()
    {
        useSkill.usingSkill.TakeEstherSkillDamage(estherSkill.estherSkillPosition.transform);
    }
}
