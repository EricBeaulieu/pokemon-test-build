using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpBeak : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.SharpBeak; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new SharpBeak(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Flying)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
