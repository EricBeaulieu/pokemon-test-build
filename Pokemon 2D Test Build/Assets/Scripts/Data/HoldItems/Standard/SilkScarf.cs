using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilkScarf : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.SilkScarf; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new SilkScarf(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Normal)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
