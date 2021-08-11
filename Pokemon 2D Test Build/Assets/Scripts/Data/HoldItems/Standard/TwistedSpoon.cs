using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistedSpoon : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.TwistedSpoon; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new TwistedSpoon(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Psychic)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
