using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntrepidSword : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.IntrepidSword; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new IntrepidSword(); }
    public override string Description()
    {
        return "Boosts the Pok�mon's Attack stat when the Pok�mon enters a battle.";
    }
    public override StatBoost OnEntryRaiseStat(Pokemon opposingPokemon)
    {
        return new StatBoost() { stat = StatAttribute.Attack, boost = 1 };
    }
}