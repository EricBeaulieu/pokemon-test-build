using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGem : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.PoisonGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new PoisonGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Poison && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
