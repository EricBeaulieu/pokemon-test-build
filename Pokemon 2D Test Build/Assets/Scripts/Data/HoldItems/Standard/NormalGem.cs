using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGem : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.NormalGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new NormalGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Normal && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
