using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Berserk; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Berserk(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Sp. Atk stat when it takes a hit that causes its HP to become half or less.";
    }
    public override StatBoost BoostStatUponCertainConditions(Pokemon defendingPokemon)
    {
        if(defendingPokemon.currentHitPoints <= (defendingPokemon.maxHitPoints/2))
        {
            return new StatBoost(StatAttribute.SpecialAttack,1);
        }
        return base.BoostStatUponCertainConditions(defendingPokemon);
    }
}
