using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackGlasses : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.BlackGlasses; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new BlackGlasses(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Dark)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
