using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.GhostGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new GhostGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Ghost && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
