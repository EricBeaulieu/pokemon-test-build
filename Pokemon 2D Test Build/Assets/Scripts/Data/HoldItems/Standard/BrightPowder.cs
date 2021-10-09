using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightPowder : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.BrightPowder; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new BrightPowder(); }
    public override MoveBase AlterOpposingMoveDetails(MoveBase move)
    {
        move = move.Clone();
        move.AdjustedMoveAccuracyPercentage(-0.1f);
        return move;
    }
}
