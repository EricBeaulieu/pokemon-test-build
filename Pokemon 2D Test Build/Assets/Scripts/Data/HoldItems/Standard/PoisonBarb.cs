using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBarb : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.PoisonBarb; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new PoisonBarb(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Poison)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
