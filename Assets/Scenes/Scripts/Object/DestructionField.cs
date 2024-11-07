using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionField : MonoBehaviour
{
    [SerializeField] private GameObject destructionEffectPrefab;
    [SerializeField] private GameObject createDestructionEffect;
    [SerializeField] private float waitTime;
    public GameObject DestructionEffectPrefab { get => destructionEffectPrefab; set => destructionEffectPrefab = value; }
    public GameObject CreateDestructionEffect { get => createDestructionEffect; set => createDestructionEffect = value; }

    private void OnEnable()
    {
        GroundManger.onDestructionGround += InstantiateDestructionEffect;
    }

    private void OnDisable()
    {
        StopCoroutine(InstantiateDestuction());
        GroundManger.onDestructionGround -= InstantiateDestructionEffect;
    }

    protected virtual void InstantiateDestructionEffect()
    {
        StartCoroutine(InstantiateDestuction());
    }

    protected IEnumerator InstantiateDestuction()
    {
        yield return new WaitForSeconds(waitTime);
        createDestructionEffect = Instantiate(destructionEffectPrefab, transform.position, transform.rotation);
    }
}
