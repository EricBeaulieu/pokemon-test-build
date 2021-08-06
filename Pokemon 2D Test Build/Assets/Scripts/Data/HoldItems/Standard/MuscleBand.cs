using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleBand : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.MuscleBand; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new MuscleBand(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.MoveType == MoveType.Physical)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
