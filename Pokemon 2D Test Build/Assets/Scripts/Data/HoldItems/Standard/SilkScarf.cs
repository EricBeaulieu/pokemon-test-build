using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilkScarf : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.SilkScarf; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Normal)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
