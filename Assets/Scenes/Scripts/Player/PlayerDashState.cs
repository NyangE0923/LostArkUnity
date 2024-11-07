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

        dashStartPosition = transform.position;                                     // ��� ���� ��ġ
        dashDirection = transform.forward;                                          // ��� ���� ����
        dashTargetPosition = dashStartPosition + dashDirection * info.DashDistance; // ��� ��ǥ ��ġ
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
            // Nav�� Move�޼ҵ�� �̵��Ϸ��� ����� �ӵ��� ������� �̵� ���͸� �����.
            // ��� Ž�� ����� ������� �����鼭 �������� �̵���Ű�� ���
            // ������Ʈ�� ��� ������ �����Ǹ鼭 �� �����Ӹ��� ������ ������ ��ŭ �̵��� �� �ְ� ����.
            // move = ����.nomalized * �ӵ� * Time.deltaTime
            Vector3 move = dashDirection.normalized * info.DashSpeed * Time.deltaTime;
            info.Nav.Move(move);
            // �̵��� ��ġ�� ����ȭ
            info.Nav.Warp(transform.position);

            // �ڽ��� ��ġ�� ��ǥ ��� ��ġ�� 0.2 ���� �Ǵ� �������� ���� true�� ��ȯ�� ���
            if (Vector3.Distance(transform.position, dashTargetPosition) <= 0.2f || DetectObstacle())
            {
                // �ڽ��� ��ġ�� ����ȭ �ϰ� Idle���·� �Ѿ��.
                info.Nav.Warp(transform.position);
                stateMachine.ChangeState(PlayerStateMachine.PLAYERSTATE.IDLE);
            }
        }
    }

    // ����ĳ��Ʈ�� ���� ��ֹ��� �ִ��� Ȯ���ϰ� bool���� ��ȯ�ϴ� �޼ҵ�
    private bool DetectObstacle()
    {
        // ���� �浹 Ž��, �ݶ��̴� �迭�� ��Ƽ� foreach�� ���� ���������� �ݺ��Ѵ�.
        // �̶� ��� ��ü�� ��ֹ� �Ǵ� �� �±׸� ������ �ִٸ� true�� ��ȯ�Ѵ�.
        // �⺻������ false�� ��ȯ�ϰ� �ִ�.
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

    // ���� �浹 Ž���� ������ Ȯ�� �� �� �ִ� �����
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
