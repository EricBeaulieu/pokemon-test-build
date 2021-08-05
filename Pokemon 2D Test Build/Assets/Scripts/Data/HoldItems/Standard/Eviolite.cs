using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eviolite : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.Eviolite; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Eviolite(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Fairy && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
