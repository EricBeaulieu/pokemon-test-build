using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticWater : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.MysticWater; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Water)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
