using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBarb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.PoisonBarb; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Poison)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
