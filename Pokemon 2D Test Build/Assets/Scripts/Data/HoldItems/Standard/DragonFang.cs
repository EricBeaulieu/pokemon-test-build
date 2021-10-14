using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFang : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.DragonFang; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Dragon)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder,move);
    }
}
