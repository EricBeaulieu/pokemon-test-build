using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.FlyingGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new FlyingGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Flying && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
