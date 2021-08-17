using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelGem : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.SteelGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new SteelGem(); }
    public override bool PlayAnimationWhenUsed() { return true; }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Steel && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
