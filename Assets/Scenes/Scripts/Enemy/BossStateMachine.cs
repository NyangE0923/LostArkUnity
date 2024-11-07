using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    public enum BOSSSTATE { IDLE, DETECT, GROGGY, ATTACK, COUNTER, STUN, DIE, SPAWN, DESTRUCTIONGROUND, BLOODSACRIFICE}
    [SerializeField] private BossState currentState;
    [SerializeField] private BossState[] bossStates;
    [SerializeField] private BossState[] bossStates2;

    //public enum WAVE { FIRST, SECOND }

    //private Dictionary<WAVE, BossState[]> waveStateMap;

    private GameObject player;
    public GameObject Player { get => player; set => player = value; }

    //public WAVE currentWave = WAVE.FIRST;

    protected virtual void Start()
    {
        //waveStateMap = new Dictionary<WAVE, BossState[]>();
        //waveStateMap[WAVE.FIRST] = bossStates;
        //waveStateMap[WAVE.SECOND] = bossStates2;

        //// 예. 웨이브가 2페이즈로 변경됨
        //currentWave = WAVE.SECOND;

        //waveStateMap[currentWave] = bossStates;

        Player = GameObject.FindWithTag("Player");
        ChangeState(BOSSSTATE.IDLE);
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeState(BOSSSTATE state)
    {
        currentState?.ExitState();
        currentState = bossStates[(int)state];
        currentState.EnterState(state);
    }
}
