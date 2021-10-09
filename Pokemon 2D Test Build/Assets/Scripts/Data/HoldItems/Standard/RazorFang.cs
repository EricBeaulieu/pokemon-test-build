using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorFang : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.RazorFang; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new RazorFang(); }
    MoveSecondaryEffects secondaryEffects = new MoveSecondaryEffects(ConditionID.Flinch, 10);
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        move = move.Clone();
        move.AddSecondaryEffects(secondaryEffects);
        return move;
    }
}
