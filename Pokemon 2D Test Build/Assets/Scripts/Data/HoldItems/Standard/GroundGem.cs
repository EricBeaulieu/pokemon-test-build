using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.GroundGem; } }
    public override bool PlayAnimationWhenUsed() { return true; }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Ground && move.MoveType != MoveType.Status)
        {
            holder.removeItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
