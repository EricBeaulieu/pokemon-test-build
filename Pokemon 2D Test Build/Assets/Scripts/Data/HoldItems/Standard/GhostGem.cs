using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.GhostGem; } }
    public override bool PlayAnimationWhenUsed() { return true; }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Ghost && move.MoveType != MoveType.Status)
        {
            holder.removeItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
