using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.Metronome; } }
    float moveBonus;
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        moveBonus = 0.2f * holder.lastMoveUsedConsecutively;
        moveBonus = Mathf.Clamp01(moveBonus);

        move = move.Clone();
        move.AdjustedMovePower(moveBonus);

        return base.AlterUserMoveDetails(holder, move);
    }
}
