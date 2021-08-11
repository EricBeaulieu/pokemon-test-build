using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverPowder : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.SilverPowder; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new SilverPowder(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Bug)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
