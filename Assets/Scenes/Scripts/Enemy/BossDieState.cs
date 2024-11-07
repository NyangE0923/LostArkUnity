using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDieState : BossState
{
    [SerializeField] private GameObject ValtanObject;
    [SerializeField] private GameObject ghostValtanPrefab;
    [SerializeField] private GameObject ghostValtanObject;
    [SerializeField] private float activeTimer;
    [SerializeField] private float activeTime;
    [SerializeField] private bool ghostSpawning;

    public override void EnterState(BossStateMachine.BOSSSTATE state)
    {
        base.EnterState(state);
        nav.isStopped = true;
        nav.Warp(transform.position);
        info.Anim.SetInteger("State", (int)state);
        info.IsDie = true;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        activeTimer += Time.deltaTime;
        if(activeTimer >= activeTime)
        {
            if(!ghostSpawning && ValtanObject)
            {
                ghostValtanObject = Instantiate(ghostValtanPrefab, transform.position, transform.rotation);
                ghostSpawning = true;
                Destroy(ValtanObject, 3f);
            }
        }
    }
}
