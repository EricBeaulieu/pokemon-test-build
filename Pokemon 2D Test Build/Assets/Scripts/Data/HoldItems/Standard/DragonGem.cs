using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.DragonGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new DragonGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Dragon && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
