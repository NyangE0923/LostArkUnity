using UnityEngine;
using UnityEngine.UI;

public class PlayerEstherUI : MonoBehaviour
{
    public Image estherSkillIcon;
    public GameObject estherGaugeLight;
    public Slider estherGaugeSlider;
    public PlayerUseEstherSkill estherSkill;

    private void Awake()
    {
        estherSkill = FindObjectOfType<PlayerUseEstherSkill>();
    }

    private void Update()
    {
        float estherGauge = (float)estherSkill.estherGauge / estherSkill.maxEstherGauge;
        estherGaugeSlider.value = Mathf.Clamp(estherGauge, 0, 1);

        UpdateEstherSkillUI();
    }

    public void UpdateEstherSkillUI()
    {
        if (estherSkill.estherGauge >= estherSkill.maxEstherGauge)
        {
            estherGaugeLight.SetActive(true);
            estherSkillIcon.gameObject.SetActive(false);
        }
        else
        {
            estherGaugeLight.SetActive(false);
            estherSkillIcon.gameObject.SetActive(true);
        }
    }
}
