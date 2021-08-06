using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleSeed : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.MiracleSeed; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new MiracleSeed(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Grass)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
