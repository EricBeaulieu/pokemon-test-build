using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.NormalGem; } }
    public override bool PlayAnimationWhenUsed() { return true; }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Normal && move.MoveType != MoveType.Status)
        {
            holder.removeItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
