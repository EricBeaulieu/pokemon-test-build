using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideLens : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.WideLens; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        move = move.Clone();
        move.AdjustedMoveAccuracyPercentage(0.1f);
        return move;
    }
}
