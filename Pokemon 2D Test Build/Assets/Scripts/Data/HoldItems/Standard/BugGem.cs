using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugGem : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.BugGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new BugGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Bug && move.MoveType != MoveType.Status)
        {
            RemoveItem();
            move = move.Clone();
            move.adjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
