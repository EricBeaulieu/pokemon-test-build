using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DauntlessShield : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.DauntlessShield; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new DauntlessShield(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Defense stat when the Pokémon enters a battle.";
    }
    public override StatBoost OnEntryRaiseStat(Pokemon opposingPokemon)
    {
        return new StatBoost() { stat = StatAttribute.Defense, boost = 1 };
    }
}
