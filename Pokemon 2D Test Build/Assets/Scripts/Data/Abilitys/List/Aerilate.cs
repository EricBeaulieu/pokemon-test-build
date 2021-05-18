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
    public override MoveBase ChangeMovesToDifferentTypeAndIncreasesTheirPower(MoveBase move)
    {
        if (move.Type == ElementType.Normal)
        {
            return move.adjustedMove(ElementType.Flying, 0.2f);
        }
        return base.ChangeMovesToDifferentTypeAndIncreasesTheirPower(move);
    }
}
