using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    Item currentItem;
    [SerializeField] Image standardBackground;
    [SerializeField] GameObject background;
    [SerializeField] GameObject selector;
    [SerializeField] Image itemSprite;
    [SerializeField] Text itemName;
    [SerializeField] Text itemCount;
    [SerializeField] Text messageBox;

    public void SetData(Item item,Color missingColor)
    {
        currentItem = item;

        if(item == null)
        {
            standardBackground.color = missingColor;
            background.SetActive(false);
            itemSprite.color = Color.clear;
            itemName.text = "";
            itemCount.text = "";
            return;
        }
        standardBackground.color = Color.white;
        background.SetActive(true);
        itemSprite.color = Color.white;
        itemSprite.sprite = item.ItemBase.ItemSprite;
        itemName.text = $"{item.ItemBase.ItemName}";
        itemCount.text = $"x {item.Count}";
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetMessageText(currentItem);
        InventorySystem.SetLastItemButton(this);
        EnableSelector(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        EnableSelector(false);
    }

    void EnableSelector(bool enabled)
    {
        selector.SetActive(enabled);
    }

    void SetMessageText(Item item)
    {
        if(item == null)
        {
            messageBox.text = "";
        }
        else
        {
            messageBox.text = item.ItemBase.ItemDescription;
        }
    }
}
