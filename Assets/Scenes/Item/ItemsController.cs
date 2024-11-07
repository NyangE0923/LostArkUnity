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
        // 쿨타임, 아이템 갯수를 확인하고 true를 반환한다면
        if (useItem.CanUseItem_CoolTime(itemIndex) && useItem.CanUseItem_ItemCount(itemIndex))
        {
            useItem.SelectItem(itemIndex);

            if (useItem.usingItem!= null)
            {
                // 회복 아이템이라면 회복 메소드 호출(플레이어 회복)
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
