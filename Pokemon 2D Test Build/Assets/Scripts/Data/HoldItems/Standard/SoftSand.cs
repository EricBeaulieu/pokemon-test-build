using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftSand : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.SoftSand; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Ground)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
