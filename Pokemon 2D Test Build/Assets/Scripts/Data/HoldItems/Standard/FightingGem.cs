using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.FightingGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new FightingGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Fighting && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
