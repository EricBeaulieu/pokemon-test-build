using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiseGlasses : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.WiseGlasses; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new WiseGlasses(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.MoveType == MoveType.Special)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
