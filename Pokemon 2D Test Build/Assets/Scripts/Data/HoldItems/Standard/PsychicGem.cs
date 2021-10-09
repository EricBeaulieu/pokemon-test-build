using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicGem : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.PsychicGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new PsychicGem(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Psychic && move.MoveType != MoveType.Status)
        {
            RemoveItem = true;
            move = move.Clone();
            move.AdjustedMovePower(0.5f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
