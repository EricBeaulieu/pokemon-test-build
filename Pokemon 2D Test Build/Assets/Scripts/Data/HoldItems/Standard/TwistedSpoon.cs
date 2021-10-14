using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistedSpoon : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.TwistedSpoon; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Psychic)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
