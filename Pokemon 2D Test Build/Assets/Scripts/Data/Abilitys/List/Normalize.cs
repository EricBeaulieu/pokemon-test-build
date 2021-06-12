using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normalize : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Normalize; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Normalize(); }
    public override string Description()
    {
        return "All the Pokémon's moves become Normal type. The power of those moves is boosted a little.";
    }
    public override MoveBase ChangeMovesToDifferentTypeAndIncreasesTheirPower(MoveBase move)
    {
        return move.adjustedMovePower(ElementType.Normal, 0.2f);
    }
}
