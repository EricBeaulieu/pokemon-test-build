using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTag : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.SpellTag; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new SpellTag(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Ghost)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
