using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseEstherSkill : MonoBehaviour
{
    [Header("Esther")]
    public int estherGauge;
    public int maxEstherGauge;
    public int estherGaugeCharge;
    // 카운터를 성공했다면 출력될 bool값을 이용해서 게이지를 회복시킬 변수
    public int counterEstherGaugeCharge;
    public float estherGaugeChargeTimer;
    public bool estherGaugeChargeCheck;
    public GameObject estherSkillPositionPrefab;
    public GameObject estherSkillPosition;
    public LayerMask groundLayer;

    private PlayerUseSkill useSkill;

    private void Awake()
    {
        estherGauge = 0;
        useSkill = GetComponent<PlayerUseSkill>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (!estherGaugeChargeCheck && estherGauge < maxEstherGauge)
        {
            StartCoroutine(EstherGaugeCharge());
        }
    }

    private IEnumerator EstherGaugeCharge()
    {
        estherGaugeChargeCheck = true;
        yield return new WaitForSeconds(estherGaugeChargeTimer);
        if(estherGauge < maxEstherGauge)
        {
            estherGauge += estherGaugeCharge;
            if(estherGauge > maxEstherGauge)
            {
                estherGauge = maxEstherGauge;
            }
        }
        estherGaugeChargeCheck = false;
    }

    public void RecoverEstherOnCounter()
    {
        estherGauge += counterEstherGaugeCharge;
    }

    public void EstherSkillPositionCreate()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Quaternion prefabRotation = estherSkillPositionPrefab.transform.rotation;
        Quaternion totalRotation = transform.rotation * prefabRotation;

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            estherSkillPosition = Instantiate(estherSkillPositionPrefab, hit.point, totalRotation);
        }
    }

    public bool EstherGaugeCheck()
    {
        return estherGauge >= maxEstherGauge;
    }
}
