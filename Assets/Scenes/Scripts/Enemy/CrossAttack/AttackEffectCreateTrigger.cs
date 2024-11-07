using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectCreateTrigger : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject attackEffectPrefab;
    [SerializeField] private GameObject attackEffectGameObject;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void CreateAttackEffectTrigger()
    {
        attackEffectGameObject = Instantiate(attackEffectPrefab, transform.position, transform.rotation);
    }
}
