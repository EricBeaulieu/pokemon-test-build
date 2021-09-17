using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    [SerializeField] ItemBase itemBase;
    [SerializeField] int count;

    public ItemBase ItemBase
    {
        get { return itemBase; }
        set
        {
            itemBase = value;
        }
    }

    public int Count
    {
        get { return count; }
        set
        {
            count = value;
        }
    }

    public Item(ItemBase newItem,int newCount = 1)
    {
        itemBase = newItem;
        count = newCount;
    }

    public Item(ItemSaveData savedItem)
    {
        itemBase = Resources.Load<ItemBase>(savedItem.item);
        count = savedItem.count;
    }

    public ItemSaveData GetSaveData()
    {
        ItemSaveData itemSaveData = new ItemSaveData();

        itemSaveData.item = SavingSystem.GetAssetPath(itemBase);
        itemSaveData.count = count;

        return itemSaveData;
    }
}
