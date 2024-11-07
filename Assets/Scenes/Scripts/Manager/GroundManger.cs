using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManger : MonoBehaviour
{
    // 그라운드 매니저를 통해 지형의 파괴 유무 파악
    public static GroundManger instance;
    [SerializeField] private bool isDestructionWalls;
    [SerializeField] private bool isDestructionFloors;
    [SerializeField] private GameObject[] destroyableWalls;
    [SerializeField] private GameObject[] destroyableFloors;
    [SerializeField] private GameObject[] insideColliders;
    [SerializeField] private float activeFalseTime;
    [SerializeField] private float destroyTime;
    [SerializeField] private float destructionTime;

    public delegate void OnDestructionGround();
    public static OnDestructionGround onDestructionGround;

    #region Propertys
    public bool IsDestructionWalls { get => isDestructionWalls; set => isDestructionWalls = value; }
    public bool IsDestructionFloors { get => isDestructionFloors; set => isDestructionFloors = value; }
    public GameObject[] DestroyableWalls { get => destroyableWalls; set => destroyableWalls = value; }
    public GameObject[] DestroyableFloors { get => destroyableFloors; set => destroyableFloors = value; }
    public GameObject[] InsideColliders { get => insideColliders; set => insideColliders = value; }
    #endregion

    private void OnEnable()
    {
        onDestructionGround += DestructionField;
    }
    private void OnDisable()
    {
        onDestructionGround -= DestructionField;
    }


    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {

    }

    public void DestructionField()
    {
        StartCoroutine(Destruction());
        IsDestructionFloors = true;
        IsDestructionWalls = true;
    }

    public IEnumerator Destruction()
    {
        yield return new WaitForSeconds(destructionTime);
        for (int i = 0; i < destroyableFloors.Length; i++)
        {
            if (DestroyableFloors != null)
            {
                DestroyableFloors[i].SetActive(false);
            }
            yield return new WaitForSeconds(activeFalseTime);
        }

        for (int i = 0; i < DestroyableWalls.Length; i++)
        {
            if (DestroyableWalls[i] != null)
            {
                DestroyableWalls[i].SetActive(false);
            }
            yield return new WaitForSeconds(activeFalseTime);
        }

        for (int i = 0; i < InsideColliders.Length; i++)
        {
            InsideColliders[i].SetActive(true);
            yield return null;
        }

        for (int i = 0; i < DestroyableWalls.Length; i++)
        {
            Destroy(DestroyableWalls[i]);
            yield return new WaitForSeconds(destroyTime);
        }

        for (int i = 0; i < DestroyableFloors.Length; i++)
        {
            Destroy(DestroyableFloors[i]);
            yield return new WaitForSeconds(destroyTime);
        }
    }




    public bool CheckDestructionField()
    {
        return IsDestructionFloors && IsDestructionWalls;
    }
}
