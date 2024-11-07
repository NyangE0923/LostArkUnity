using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    public PlayerUseItem useItem;
    public PlayerStats stats;
    public PlayerUI playerUI;
    public PlayerInfo info;

    private void Awake()
    {
        useItem = GetComponent<PlayerUseItem>();
        stats = GetComponent<PlayerStats>();
        playerUI = FindObjectOfType<PlayerUI>();
        info = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        playerUI.UpdateItemUI(useItem.playerItems);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            useItem.CancleBombRange();
        }

        if (info.IsDie) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryUseItem(PlayerUseItem.ITEM.HEALTHPOTION);
        }

        if (info.IsFallDown) return;

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryUseItem(PlayerUseItem.ITEM.DESTRUCTIONBOMB);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (useItem.usingItem != null && useItem.usingItem.isItemAvailable)
            {
                TryUseItem(PlayerUseItem.ITEM.DESTRUCTIONBOMB);
            }
        }

    }

    private void TryUseItem(PlayerUseItem.ITEM itemIndex)
    {
        // ��Ÿ��, ������ ������ Ȯ���ϰ� true�� ��ȯ�Ѵٸ�
        if (useItem.CanUseItem_CoolTime(itemIndex) && useItem.CanUseItem_ItemCount(itemIndex))
        {
            useItem.SelectItem(itemIndex);

            if (useItem.usingItem!= null)
            {
                // ȸ�� �������̶�� ȸ�� �޼ҵ� ȣ��(�÷��̾� ȸ��)
                if (useItem.usingItem.isHealthPotion && stats.Health < stats.MaxHealth)
                {
                    useItem.SelectItemUse();
                    useItem.usingItem.RecoveryHealth(stats);
                }

                if (useItem.usingItem.isBomb)
                {
                    if (!useItem.usingItem.isItemAvailable)
                    {
                        useItem.CreateBombRange();
                        useItem.usingItem.isItemAvailable = true;
                    }
                    else
                    {
                        useItem.SelectItemUse();
                        useItem.UsingBombItem();
                    }
                }
            }
        }
    }
}
