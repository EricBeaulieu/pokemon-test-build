using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.GroundGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new GroundGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Ground && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
