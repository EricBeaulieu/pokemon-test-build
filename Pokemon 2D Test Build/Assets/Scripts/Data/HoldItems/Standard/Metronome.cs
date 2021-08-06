using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.Metronome; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Metronome(); }
    MoveBase compoundingMove;
    float moveBonus;
    public override bool PreventTheUseOfCertainMoves(BattleUnit battleUnit, MoveBase move)
    {
        if(move != compoundingMove)
        {
            compoundingMove = move;
            moveBonus = 0;
        }
        else
        {
            moveBonus += 0.2f;
            moveBonus = Mathf.Clamp01(moveBonus);
        }

        return base.PreventTheUseOfCertainMoves(battleUnit, move);
    }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        move = move.Clone();
        move.AdjustedMovePower(moveBonus);

        return base.AlterUserMoveDetails(move);
    }
}
