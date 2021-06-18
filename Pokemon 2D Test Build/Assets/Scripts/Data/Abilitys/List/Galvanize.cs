using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galvanize : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Galvanize; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Galvanize(); }
    public override string Description()
    {
        return "Normal-type moves become Electric-type moves. The power of those moves is boosted a little.";
    }
    public override MoveBase AlterMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Normal)
        {
            move = move.Clone();
            move.adjustedMoveType(ElementType.Electric);
            move.adjustedMovePower(0.2f);
        }
        return base.AlterMoveDetails(move);
    }
}
