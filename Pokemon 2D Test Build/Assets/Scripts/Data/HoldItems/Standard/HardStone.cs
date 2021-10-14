using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardStone : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.HardStone; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Rock)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
}
