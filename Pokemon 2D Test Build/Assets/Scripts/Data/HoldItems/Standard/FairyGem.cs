using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyGem : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.FairyGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new FairyGem(); }
    public override bool PlayAnimationWhenUsed() { return true; }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Fairy && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}