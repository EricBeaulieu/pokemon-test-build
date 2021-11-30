using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, ISelectHandler
{
    [SerializeField] Text nameText;
    [SerializeField] Text costText;

    ItemBase itemSold;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener( () => GameManager.instance.GetShopSystem.SelectItemToPurchase(itemSold));
    }

    public void SetupData(ItemBase item)
    {
        if(item == null)
        {
            nameText.text = "-----";
            costText.text = $"$---";
            itemSold = null;
            return;
        }
        nameText.text = item.ItemName;
        costText.text = $"${item.GetItemValue.ToString()}";
        itemSold = item;
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShopSystem.itemDetails.SetUpData(itemSold);
        GameManager.instance.GetShopSystem.HandleScrolling(this);
    }

}
