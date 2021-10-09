using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.FireGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new FireGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Fire && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
