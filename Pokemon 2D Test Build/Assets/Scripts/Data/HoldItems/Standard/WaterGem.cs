using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGem : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.WaterGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new WaterGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Water && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
