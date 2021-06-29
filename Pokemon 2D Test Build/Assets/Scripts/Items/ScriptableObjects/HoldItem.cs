using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItem : ItemBase
{
    [Header("Hold Item Attributes")]
    [SerializeField] HoldItemID holdItemID;

    public override HoldItemBase HoldItemAffects()
    {
        return HoldItemDB.GetHoldItem(holdItemID);
    }

    public override void UseItem()
    {
        throw new System.NotImplementedException();
    }

}
