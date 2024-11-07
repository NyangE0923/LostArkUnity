using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    PlayerStats stats;
    PlayerInfo info;
    // Ui에서 사용할 이미지
    public Image[] skillIcons;
    public Image[] itemIcons;
    public Image dashIcon;
    // ui에서 사용할 텍스트
    public TMP_Text[] skillCoolDownTexts;
    public TMP_Text[] itemCoolDownTexts;
    public TMP_Text[] itemCountTexts;
    public Slider manaSlider;
    public Slider healthSlider;

    public float uiDashCoolTimer;
    public GameObject dashIconGameObject;

    private void Awake()
    {
        stats = FindObjectOfType<PlayerStats>();
        info = FindObjectOfType<PlayerInfo>();
    }

    private void Update()
    {
        if(manaSlider != null)
        {
            float manaPoint = stats.ManaPoint / stats.MaxManaPoint;
            manaSlider.value = Mathf.Clamp(manaPoint, 0f, 1f);
        }

        if(healthSlider != null)
        {
            float health = (float)stats.Health / stats.MaxHealth;
            healthSlider.value = Mathf.Clamp(health, 0f, 1f);
        }
    }

    public void UpdateSkillUI(SkillBase[] skills)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            // skill 변수에 배열 값을 넣는다.
            SkillBase skill = skills[i];
            // icons 배열이 i보다 초과한다면
            if (skillIcons.Length > i)
            {
                // icons의 i번째 sprite에 skill의 Icon(스프라이트)을 넣는다. 
                skillIcons[i].sprite = skill.Icon;
                // 쿨타이머와 쿨타임을 비교해서 쿨타이머가 쿨타임보다 미만이라면
                if(skill.skillCoolTimer < skill.skillCoolTime)
                {
                    // i번째 icon의 fillAmont의 값을 1 - 쿨타이머와 쿨타임을 비례한 값으로 한다 (0.0 ~ 1.0)
                    skillIcons[i].fillAmount = 1 - (skill.skillCoolTimer / skill.skillCoolTime);
                }
                else
                {
                    // 남은 시간이 없다면 이미지의 fillAmount 값을 0으로 한다.
                    skillIcons[i].fillAmount = 0f;
                }
            }

            if (skillCoolDownTexts.Length > i)
            {
                if (skill.skillCoolTimer < skill.skillCoolTime)
                {
                    // 텍스트 오브젝트 활성화
                    // 남은 시간을 스킬 쿨타임과 타이머로 계산하고
                    // 남은 시간을 반올림해서 정수로 표기한다.
                    skillCoolDownTexts[i].gameObject.SetActive(true);
                    float remainingTime = skill.skillCoolTime - skill.skillCoolTimer;
                    skillCoolDownTexts[i].text = Mathf.RoundToInt(remainingTime).ToString() + "s";
                }
                else
                {
                    // 남은 시간이 없다면 텍스트 오브젝트 비활성화
                    skillCoolDownTexts[i].gameObject.SetActive(false);
                }
            }
        }

        if (info.IsDashing)
        {
            uiDashCoolTimer += Time.deltaTime;
            dashIconGameObject.gameObject.SetActive(true);
            dashIcon.fillAmount = 1 - (uiDashCoolTimer / info.DashSkillCoolTime);
        }
        else
        {
            uiDashCoolTimer = 0;
            dashIconGameObject.gameObject.SetActive(false);
        }
    }

    public void UpdateItemUI(ItemBase[] items)
    {
        for(int i = 0; i < items.Length; i++)
        {
            ItemBase item = items[i];

            if(itemIcons.Length > i)
            {
                itemIcons[i].sprite = item.itemIcon;

                if(item.itemCoolTimer < item.itemCoolTime)
                {
                    itemIcons[i].fillAmount = 1 - (item.itemCoolTimer / item.itemCoolTime);
                }
                else
                {
                    // 남은 시간이 없다면 이미지의 fillAmount 값을 0으로 한다.
                    itemIcons[i].fillAmount = 0f;
                }
            }

            if (itemCoolDownTexts.Length > i)
            {
                if (item.itemCoolTimer < item.itemCoolTime)
                {
                    // 텍스트 오브젝트 활성화
                    // 남은 시간을 스킬 쿨타임과 타이머로 계산하고
                    // 남은 시간을 반올림해서 정수로 표기한다.
                    itemCoolDownTexts[i].gameObject.SetActive(true);
                    float remainingTime = item.itemCoolTime - item.itemCoolTimer;
                    itemCoolDownTexts[i].text = Mathf.RoundToInt(remainingTime).ToString() + "s";
                }
                else
                {
                    // 남은 시간이 없다면 텍스트 오브젝트 비활성화
                    itemCoolDownTexts[i].gameObject.SetActive(false);
                }
            }

            if (itemCountTexts.Length > i)
            {
                itemCountTexts[i].text = item.itemCount.ToString();

                if(item.itemCount == 0)
                {
                    itemIcons[i].fillAmount = 1f;
                }
            }
        }
    }
}
