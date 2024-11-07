using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] private Slider bossHealthBar;
    [SerializeField] private Slider bossGroggyGaugeBar;
    [SerializeField] private GameObject bossBuff;
    [SerializeField] private Text bossBuffText;
    private BossStats bossStats;

    void Start()
    {
        bossStats = FindObjectOfType<BossStats>();
    }

    void Update()
    {
        if (bossStats == null) return;

        if (bossHealthBar != null)
        {
            float bossHealth = (float)bossStats.Health / bossStats.MaxHealth;
            bossHealthBar.value = Mathf.Clamp(bossHealth, 0, 1);
        }
        if (bossGroggyGaugeBar != null)
        {
            float bossGroggyGauge = (float)bossStats.GroggyGauge / bossStats.MaxGroggyGauge;
            bossGroggyGaugeBar.value = Mathf.Clamp(bossGroggyGauge, 0, 1);
        }

        if (bossStats.ArmorCount > 0)
        {
            bossBuff.SetActive(true);
            bossBuffText.text = "X " + bossStats.ArmorCount.ToString();
        }
        else
        {
            bossBuff.SetActive(false);
        }
    }

    public void InitGhostForm(BossStats stats)
    {
        bossStats = stats;
    }
}
