using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGem : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.IceGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new IceGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Ice && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
