using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LustrousOrb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LustrousOrb; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LustrousOrb(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Dragon || move.Type == ElementType.Water)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
