using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorMove : MonoBehaviour
{
    public LayerMask groundLayer;
    public float heightAboveGround = 0.1f;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPos = hit.point;
            targetPos.y += heightAboveGround;
            transform.position = targetPos;
        }
    }
}
