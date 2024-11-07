using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    public enum ITEM { HEALTHPOTION, DESTRUCTIONBOMB }
    public ItemBase usingItem;
    public ItemBase[] playerItems;
    public GameObject[] itemEffects;

    public GameObject itemRangePrefab;
    public GameObject itemBombEffect;

    public void Start()
    {
        for(int i = 0; i < playerItems.Length; i++)
        {
            playerItems[i].ItemCoolTimeInitialize();
            playerItems[i].isItemAvailable = false;
        }
    }

    public void Update()
    {
        for(int i = 0; i < playerItems.Length; i++)
        {
            if(playerItems[i].itemCoolTimer <= playerItems[i].itemCoolTime)
            {
                playerItems[i].itemCoolTimer += Time.deltaTime;
            }  
        }
    }

    public void SelectItem(ITEM itemIndex)
    {
        usingItem = playerItems[(int)itemIndex];
    }

    public void SelectItemUse()
    {
        usingItem.UseItem();
    }

    public bool CanUseItem_CoolTime(ITEM itemIndex)
    {
        return playerItems[(int)itemIndex].itemCoolTimer >= playerItems[(int)itemIndex].itemCoolTime;
    }

    public bool CanUseItem_ItemCount(ITEM itemIndex)
    {
        return playerItems[(int)itemIndex].itemCount > 0;
    }

    // ó�� ��ź ���� ����
    public void CreateBombRange()
    {
        // ���콺�� ��ġ�� ��������
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // ȸ���� �������� ȸ���� + �÷��̾� ��ü�� ȸ���� ��������
        Quaternion prefabRotation = usingItem.rangePrefab.transform.rotation;
        Quaternion totalRotation = transform.rotation * prefabRotation;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 prefabTransform = usingItem.rangePrefab.transform.position;
            Vector3 totalTransform = hit.point + prefabTransform;

            itemRangePrefab = Instantiate(usingItem.rangePrefab, totalTransform, totalRotation, transform);
        }
    }

    // ��ź ���� ������ ����
    public void CancleBombRange()
    {
        if(itemRangePrefab != null)
        {
            Destroy(itemRangePrefab);
            usingItem.isItemAvailable = false;
        }
    }

    public void UsingBombItem()
    {
        itemBombEffect = Instantiate(usingItem.itemEffect, itemRangePrefab.transform.position, itemRangePrefab.transform.rotation);
        usingItem.ArmorDestruction(itemBombEffect);
        CancleBombRange();
    }

    protected void OnDrawGizmos()
    {
        if (usingItem != null && usingItem.isBomb && itemRangePrefab != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(itemRangePrefab.transform.position, usingItem.destructionRadius);
        }
    }
}
