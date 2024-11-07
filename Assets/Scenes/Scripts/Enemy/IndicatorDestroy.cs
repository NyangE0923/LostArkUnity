using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorDestroy : MonoBehaviour
{
    [SerializeField] private float indicatorDuration;
    public void Start()
    {
        Destroy(gameObject, indicatorDuration);
    }
}
