using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBelt : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.BlackBelt; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Fighting)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder,move);
    }
}
