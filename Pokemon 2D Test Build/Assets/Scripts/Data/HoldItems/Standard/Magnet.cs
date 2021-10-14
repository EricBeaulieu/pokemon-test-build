using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.Magnet; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Electric)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
