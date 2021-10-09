using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.RockGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new RockGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Rock && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
