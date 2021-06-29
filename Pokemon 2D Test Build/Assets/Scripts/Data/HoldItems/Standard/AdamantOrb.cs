using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamantOrb : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.AdamantOrb; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new AdamantOrb(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Dragon || move.Type == ElementType.Steel)
        {
            move = move.Clone();
            move.adjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
