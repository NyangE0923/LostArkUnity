using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "CreateItem")]
public class ItemBase : ScriptableObject, IHealingSystem
{
    [Header("#ITEM INFO")]
    public int itemCount;
    public int maxitemCount;
    public float itemCoolTime;
    public float itemCoolTimer;
    public Sprite itemIcon;
    [Space]
    [Header("#ITEM TYPE")]
    public bool isHealthPotion;
    public bool isBomb;
    [Space]
    [Header("#HealthPotion")]
    public float recoveryHealth;
    [Space]
    [Header("#DestructionBomb")]
    public GameObject rangePrefab;
    public GameObject itemEffect;
    public bool isItemAvailable;
    public float destructionRadius;
    public LayerMask enemyLayer;

    public void ItemCoolTimeInitialize()
    {
        itemCoolTimer = itemCoolTime;
        itemCount = maxitemCount;
    }

    public void UseItem()
    {
        itemCount--;
        itemCoolTimer = 0;
    }

    public void RecoveryHealth(EntityStats stats)
    {
        float totalRecovery = stats.MaxHealth * recoveryHealth;

        if (stats.Health < stats.MaxHealth)
        {
            stats.Health += (int)totalRecovery;

            if (stats.Health >= stats.MaxHealth)
            {
                stats.Health = stats.MaxHealth;
            }
        }
    }

    public void ArmorDestruction(GameObject itemBombEffect)
    {
        Collider[] colliders = Physics.OverlapSphere(itemBombEffect.transform.position, destructionRadius, enemyLayer);

        foreach (Collider collider in colliders)
        {
            if(collider != null)
            {
                BossStats stats = collider.GetComponent<BossStats>();
                stats.DestructionArmor();
            }
        }
    }
}
