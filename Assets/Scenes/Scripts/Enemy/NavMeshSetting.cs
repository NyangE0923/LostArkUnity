using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshSetting : MonoBehaviour
{
    NavMeshSurface surface;

    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    private void OnEnable()
    {
        GroundManger.onDestructionGround += UpdateNavMeshOnDestruction;
    }

    private void OnDisable()
    {
        GroundManger.onDestructionGround -= UpdateNavMeshOnDestruction;
    }

    protected void UpdateNavMeshOnDestruction()
    {
        surface.enabled = !surface.enabled;
        Debug.Log("네브매쉬 활성/비활성화");
    }
}
