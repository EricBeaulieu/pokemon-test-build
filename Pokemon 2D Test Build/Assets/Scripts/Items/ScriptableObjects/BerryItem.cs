using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create New Berry Item")]
public class BerryItem : ItemBase
{
    [Header("Hold Item Attributes")]
    [SerializeField] BerryID berryID;

    public BerryItem()
    {
        itemType = itemType.Berry;
    }

    public override HoldItemBase HoldItemAffects()
    {
        return HoldItemDB.GetHoldItem(berryID);
    }

    public override bool UseItem(Pokemon pokemon)
    {
        return HoldItemDB.GetHoldItem(berryID).UsedInInventoryEffect(pokemon);
    }

    public override bool UseItemOption()
    {
        return HoldItemDB.GetHoldItem(berryID).UseInInventory();
    }

    public override string ItemName
    {
        get { return $"{base.ItemName} Berry"; }
    }
}