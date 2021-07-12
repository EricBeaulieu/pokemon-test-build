using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOptionDetailsUI : MonoBehaviour
{
    [SerializeField] Image itemSprite;
    [SerializeField] Text itemName;
    [SerializeField] Text itemCount;

    public void SetData(Item item)
    {
        itemSprite.sprite = item.ItemBase.ItemSprite;
        itemName.text = $"{item.ItemBase.ItemName}";
        itemCount.text = $"x {item.Count}";
    }
}
