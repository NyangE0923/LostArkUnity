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
    // Ui���� ����� �̹���
    public Image[] skillIcons;
    public Image[] itemIcons;
    public Image dashIcon;
    // ui���� ����� �ؽ�Ʈ
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
            // skill ������ �迭 ���� �ִ´�.
            SkillBase skill = skills[i];
            // icons �迭�� i���� �ʰ��Ѵٸ�
            if (skillIcons.Length > i)
            {
                // icons�� i��° sprite�� skill�� Icon(��������Ʈ)�� �ִ´�. 
                skillIcons[i].sprite = skill.Icon;
                // ��Ÿ�̸ӿ� ��Ÿ���� ���ؼ� ��Ÿ�̸Ӱ� ��Ÿ�Ӻ��� �̸��̶��
                if(skill.skillCoolTimer < skill.skillCoolTime)
                {
                    // i��° icon�� fillAmont�� ���� 1 - ��Ÿ�̸ӿ� ��Ÿ���� ����� ������ �Ѵ� (0.0 ~ 1.0)
                    skillIcons[i].fillAmount = 1 - (skill.skillCoolTimer / skill.skillCoolTime);
                }
                else
                {
                    // ���� �ð��� ���ٸ� �̹����� fillAmount ���� 0���� �Ѵ�.
                    skillIcons[i].fillAmount = 0f;
                }
            }

            if (skillCoolDownTexts.Length > i)
            {
                if (skill.skillCoolTimer < skill.skillCoolTime)
                {
                    // �ؽ�Ʈ ������Ʈ Ȱ��ȭ
                    // ���� �ð��� ��ų ��Ÿ�Ӱ� Ÿ�̸ӷ� ����ϰ�
                    // ���� �ð��� �ݿø��ؼ� ������ ǥ���Ѵ�.
                    skillCoolDownTexts[i].gameObject.SetActive(true);
                    float remainingTime = skill.skillCoolTime - skill.skillCoolTimer;
                    skillCoolDownTexts[i].text = Mathf.RoundToInt(remainingTime).ToString() + "s";
                }
                else
                {
                    // ���� �ð��� ���ٸ� �ؽ�Ʈ ������Ʈ ��Ȱ��ȭ
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
                    // ���� �ð��� ���ٸ� �̹����� fillAmount ���� 0���� �Ѵ�.
                    itemIcons[i].fillAmount = 0f;
                }
            }

            if (itemCoolDownTexts.Length > i)
            {
                if (item.itemCoolTimer < item.itemCoolTime)
                {
                    // �ؽ�Ʈ ������Ʈ Ȱ��ȭ
                    // ���� �ð��� ��ų ��Ÿ�Ӱ� Ÿ�̸ӷ� ����ϰ�
                    // ���� �ð��� �ݿø��ؼ� ������ ǥ���Ѵ�.
                    itemCoolDownTexts[i].gameObject.SetActive(true);
                    float remainingTime = item.itemCoolTime - item.itemCoolTimer;
                    itemCoolDownTexts[i].text = Mathf.RoundToInt(remainingTime).ToString() + "s";
                }
                else
                {
                    // ���� �ð��� ���ٸ� �ؽ�Ʈ ������Ʈ ��Ȱ��ȭ
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
