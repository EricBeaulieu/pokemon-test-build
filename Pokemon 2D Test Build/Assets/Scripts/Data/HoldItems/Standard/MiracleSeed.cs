using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleSeed : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.MiracleSeed; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Grass)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
