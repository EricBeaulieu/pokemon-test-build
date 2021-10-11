using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create New Hold Item")]
public class HoldItem : ItemBase
{
    [Header("Hold Item Attributes")]
    [SerializeField] HoldItemID holdItemID;

    public HoldItem()
    {
        itemType = itemType.Basic;
    }

    public override HoldItemBase HoldItemAffects()
    {
        return HoldItemDB.GetHoldItem(holdItemID);
    }

    public override bool UseItem(Pokemon pokemon)
    {
        return false;
    }

    public override bool UseItemOption()
    {
        return false;
    }
}
