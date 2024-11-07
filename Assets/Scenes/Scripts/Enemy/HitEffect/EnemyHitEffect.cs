using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitEffect : MonoBehaviour
{
    [SerializeField] private HighlightEffect hitEffect;

    private void Awake()
    {
        hitEffect = GetComponentInParent<HighlightEffect>();
    }

    public void EnemyHit()
    {
        if(hitEffect != null)
        {
            hitEffect.HitFX();
        }
    }
}
