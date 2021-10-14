using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverPowder : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.SilverPowder; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Bug)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
