using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricGem : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ElectricGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ElectricGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Electric && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
