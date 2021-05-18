using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defiant : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Defiant; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Defiant(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Attack stat sharply when its stats are lowered.";
    }
    public override StatBoost BoostStatSharplyIfAnyStatLowered()
    {
        return new StatBoost() { stat = StatAttribute.Attack, boost = 2 };
    }
}
