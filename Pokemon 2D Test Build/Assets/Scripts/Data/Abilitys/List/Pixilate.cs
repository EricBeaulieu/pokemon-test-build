using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixilate : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Pixilate; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Pixilate(); }
    public override string Description()
    {
        return "Normal-type moves become Fairy-type moves. The power of those moves is boosted a little.";
    }
    public override MoveBase AlterMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Normal)
        {
            move = move.Clone();
            move.AdjustedMoveType(ElementType.Fairy);
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterMoveDetails(move);
    }
}
