using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.DarkGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new DarkGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Dark && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
