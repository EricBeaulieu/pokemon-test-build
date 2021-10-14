using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Competitive : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Competitive; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Competitive(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Special Attack stat sharply when its stats are lowered.";
    }
    public override StatBoost BoostStatSharplyIfAnyStatLowered()
    {
        return new StatBoost(StatAttribute.SpecialAttack, 2);
    }
}
