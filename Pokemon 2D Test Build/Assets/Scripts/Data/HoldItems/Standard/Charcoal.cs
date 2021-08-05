using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charcoal : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.Charcoal; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Charcoal(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Fire)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
