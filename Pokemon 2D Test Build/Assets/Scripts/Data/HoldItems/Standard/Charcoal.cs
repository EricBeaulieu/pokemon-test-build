using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charcoal : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.Charcoal; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Fire)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder,move);
    }
}
