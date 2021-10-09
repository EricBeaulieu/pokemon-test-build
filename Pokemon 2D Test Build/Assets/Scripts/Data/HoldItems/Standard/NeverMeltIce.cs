using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverMeltIce : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.NeverMeltIce; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new NeverMeltIce(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Ice)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
