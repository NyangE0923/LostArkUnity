using Unity.AI.Navigation;
using UnityEngine;

public class ObjectStats : EntityStats
{
    [SerializeField] private float destructionTime;
    [SerializeField] private GameObject destructionEffectPrefab;
    [SerializeField] private GameObject createDestructionEffect;
    private NavMeshSurface nav;

    public GameObject DestructionEffectPrefab { get => destructionEffectPrefab; set => destructionEffectPrefab = value; }
    public GameObject CreateDestructionEffect { get => createDestructionEffect; set => createDestructionEffect = value; }

    private void Awake()
    {
        nav = GetComponentInParent<NavMeshSurface>();
    }

    protected override void Die()
    {
        base.Die();
        InstantiateDestructionEffect();
    }

    protected virtual void InstantiateDestructionEffect()
    {
        Destroy(gameObject, destructionTime);
        createDestructionEffect = Instantiate(destructionEffectPrefab, transform.position, transform.rotation);
    }

    private void OnDisable()
    {
        nav.BuildNavMesh();
    }
}
