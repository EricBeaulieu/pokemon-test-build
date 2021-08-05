using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGem : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.GrassGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new GrassGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Grass && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
