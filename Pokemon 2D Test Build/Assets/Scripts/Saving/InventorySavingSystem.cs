using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventorySavingSystem
{
    static string itemData;
    static string[] splitArray;
    static int indexPos;

    public static void SaveInventory(List<Item> inventory, string location)
    {
        itemData = "";

        for (int i = 0; i < inventory.Count; i++)
        {
            AddData(inventory[i]);
        }
        itemData = itemData.Remove(itemData.Length - 1, 1);
        Debug.Log($"Location:{location}\n{itemData}");
        PlayerPrefs.SetString(location, itemData);
    }

    public static List<Item> LoadInventorySystem(string location)
    {
        List<Item> savedInventory = new List<Item>();
        StartLoadData(location);

        ItemBase itemBase;
        int itemCount;
        Item item;

        for (int i = 0; i < splitArray.Length; i++)
        {
            itemBase = Resources.Load<ItemBase>(splitArray[i]);
            i++;
            itemCount = int.Parse(splitArray[i]);
            item = new Item() { ItemBase = itemBase, Count = itemCount };
            savedInventory.Add(item);
        }

        return savedInventory;
    }

    static void AddData(Item item)
    {
        itemData += SavingSystem.GetAssetPath(item.ItemBase);
        itemData += SavingSystem.split;
        itemData += item.Count.ToString();
        itemData += SavingSystem.split;
    }

    static void StartLoadData(string location)
    {
        location = PlayerPrefs.GetString(location);

        splitArray = location.Split(char.Parse(SavingSystem.split));
    }
}
