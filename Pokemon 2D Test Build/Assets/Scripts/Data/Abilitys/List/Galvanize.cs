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
    public override MoveBase ChangeMovesToDifferentTypeAndIncreasesTheirPower(MoveBase move)
    {
        if (move.Type == ElementType.Normal)
        {
            return move.adjustedMove(ElementType.Electric, 0.2f);
        }
        return base.ChangeMovesToDifferentTypeAndIncreasesTheirPower(move);
    }
}
