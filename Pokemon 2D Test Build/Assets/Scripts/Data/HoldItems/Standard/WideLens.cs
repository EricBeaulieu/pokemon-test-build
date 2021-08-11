using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideLens : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.WideLens; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new WideLens(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        move = move.Clone();
        move.AdjustedMoveAccuracyPercentage(0.1f);
        return move;
    }
}
