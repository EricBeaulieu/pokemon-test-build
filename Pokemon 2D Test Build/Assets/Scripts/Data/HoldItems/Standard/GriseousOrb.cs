using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GriseousOrb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.GriseousOrb; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Dragon || move.Type == ElementType.Ghost)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
