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
    public override MoveBase ChangeMovesToDifferentTypeAndIncreasesTheirPower(MoveBase move)
    {
        if (move.Type == ElementType.Normal)
        {
            return move.adjustedMove(ElementType.Ice, 0.2f);
        }
        return base.ChangeMovesToDifferentTypeAndIncreasesTheirPower(move);
    }
}
