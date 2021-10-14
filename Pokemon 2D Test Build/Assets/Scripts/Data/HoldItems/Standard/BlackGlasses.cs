using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackGlasses : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.BlackGlasses; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Dark)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder,move);
    }
}
