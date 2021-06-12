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
    public override MoveBase ChangeMovesToDifferentTypeAndIncreasesTheirPower(MoveBase move)
    {
        if (move.Type == ElementType.Normal)
        {
            return move.adjustedMovePower(ElementType.Fairy, 0.2f);
        }
        return base.ChangeMovesToDifferentTypeAndIncreasesTheirPower(move);
    }
}
