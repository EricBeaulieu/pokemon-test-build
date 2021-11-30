using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDetails : MonoBehaviour
{
    [SerializeField] Image itemSprite;
    [SerializeField] Text itemDescription;
    [SerializeField] Text inBagText;
    const string IN_BAG = "In Bag :";

    public void SetUpData(ItemBase item)
    {
        if(item == null)
        {
            itemSprite.sprite = StatusConditionArt.instance.Nothing;
            itemDescription.text = "";
            inBagText.text = IN_BAG;
            return;
        }
        itemSprite.sprite = item.ItemSprite;
        itemDescription.text = item.ItemDescription;

        int count = GameManager.instance.GetInventorySystem.CurrentInventory().Where(x => x.ItemBase == item).Sum(y => y.Count);
        inBagText.text = $"{IN_BAG} {count}";
    }
}
