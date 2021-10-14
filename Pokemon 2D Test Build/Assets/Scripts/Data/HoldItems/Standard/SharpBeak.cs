using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpBeak : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.SharpBeak; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Flying)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
