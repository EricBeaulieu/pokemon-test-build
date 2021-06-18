using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerate : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Refrigerate; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Refrigerate(); }
    public override string Description()
    {
        return "Normal-type moves become Ice-type moves. The power of those moves is boosted a little.";
    }
    public override MoveBase AlterMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Normal)
        {
            move = move.Clone();
            move.adjustedMoveType(ElementType.Ice);
            move.adjustedMovePower(0.2f);
        }
        return base.AlterMoveDetails(move);
    }
}
