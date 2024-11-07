using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private Vector3 dashDirection;
    private Vector3 dashTargetPosition;
    private Vector3 dashStartPosition;
    private GameObject dashEffectPrefab;
    [SerializeField] private GameObject detectObstaclePos;

    public override void EnterState(PlayerStateMachine.PLAYERSTATE state)
    {
        base.EnterState(state);
        info.Anim.SetInteger("State", (int)state);

        dashStartPosition = transform.position;                                     // 대시 시작 위치
        dashDirection = transform.forward;                                          // 대시 방향 벡터
        dashTargetPosition = dashStartPosition + dashDirection * info.DashDistance; // 대시 목표 위치
    }

    public override void ExitState()
    {
        if (info.IsSkill) info.IsSkill = false;
        Destroy(dashEffectPrefab, 0.7f);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (info.IsDashing)
        {
            // Nav의 Move메소드는 이동하려는 방향과 속도를 기반으로 이동 벡터를 만든다.
            // 경로 탐색 기능을 사용하지 않으면서 수동으로 이동시키는 기능
            // 에이전트의 경로 설정은 유지되면서 매 프레임마다 지정된 오프셋 만큼 이동할 수 있게 해줌.
            // move = 방향.nomalized * 속도 * Time.deltaTime
            Vector3 move = dashDirection.normalized * info.DashSpeed * Time.deltaTime;
            info.Nav.Move(move);
            // 이동한 위치값 동기화
            info.Nav.Warp(transform.position);

            // 자신의 위치와 목표 대시 위치가 0.2 이하 또는 오버랩에 의해 true를 반환할 경우
            if (Vector3.Distance(transform.position, dashTargetPosition) <= 0.2f || DetectObstacle())
            {
                // 자신의 위치를 동기화 하고 Idle상태로 넘어간다.
                info.Nav.Warp(transform.position);
                stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.IDLE);
            }
        }
    }

    // 레이캐스트를 통해 장애물이 있는지 확인하고 bool값을 반환하는 메소드
    private bool DetectObstacle()
    {
        // 구형 충돌 탐지, 콜라이더 배열에 담아서 foreach를 통해 순차적으로 반복한다.
        // 이때 담긴 객체가 장애물 또는 적 태그를 가지고 있다면 true를 반환한다.
        // 기본적으로 false를 반환하고 있다.
        Collider[] hitColliders = Physics.OverlapSphere(detectObstaclePos.transform.position, info.DetectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Obstacle") || hitCollider.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }

    // 구형 충돌 탐지를 씬에서 확인 할 수 있는 기즈모
    private void OnDrawGizmos()
    {
        if (info != null && transform != null && info.IsDashing)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectObstaclePos.transform.position, info.DetectionRadius);
        }
    }

    public void DashEffectCreate()
    {
        Quaternion prefabRotation = info.DashEffect.transform.rotation;
        Quaternion totalRotation = transform.rotation * prefabRotation;

        dashEffectPrefab = Instantiate(info.DashEffect, transform.position, totalRotation);
    }
}
