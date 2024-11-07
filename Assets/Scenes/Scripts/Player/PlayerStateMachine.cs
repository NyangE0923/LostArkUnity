using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PLAYERSTATE { IDLE, MOVE, DASH, ATTACK, SKILL, FALLDOWN, DIE}
    [SerializeField] private PlayerState currentState;
    [SerializeField] private PlayerState[] playerStates;

    public PlayerState CurrentState { get => currentState; set => currentState = value; }
    public PlayerState[] PlayerStates { get => playerStates; set => playerStates = value; }

    private void Start()
    {
        ChangeState(PLAYERSTATE.IDLE);
    }

    private void Update()
    {
        CurrentState?.UpdateState();
    }

    public void ChangeState(PLAYERSTATE state)
    {
        // 현재 상태가 null이 아니면 상태를 끝낸다.
        CurrentState?.ExitState();
        // 현재 상태를 상태배열[(int)Enum 상태];
        CurrentState = PlayerStates[(int)state];
        CurrentState.EnterState(state);
    }
}
