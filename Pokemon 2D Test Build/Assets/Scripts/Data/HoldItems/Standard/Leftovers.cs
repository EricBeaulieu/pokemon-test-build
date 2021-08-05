using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leftovers : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.Leftovers; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Leftovers(); }
    public override int EndTurnHolderAlterHp(Pokemon holder)
    {
        return Mathf.FloorToInt(holder.maxHitPoints / 16);
    }
}
