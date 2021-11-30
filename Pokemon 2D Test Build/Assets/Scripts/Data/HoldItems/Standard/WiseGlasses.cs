using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiseGlasses : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.WiseGlasses; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.MoveType == MoveType.Special)
        {
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
