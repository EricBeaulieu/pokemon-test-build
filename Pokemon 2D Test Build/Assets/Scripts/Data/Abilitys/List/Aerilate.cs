using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aerilate : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Aerilate; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Aerilate(); }
    public override string Description()
    {
        return "Normal-type moves become Flying-type moves. The power of those moves is boosted a little.";
    }
    public override MoveBase AlterMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Normal)
        {
            move = move.Clone();
            move.adjustedMoveType(ElementType.Flying);
            move.adjustedMovePower(0.2f);
        }
        return base.AlterMoveDetails(move);
    }
}
