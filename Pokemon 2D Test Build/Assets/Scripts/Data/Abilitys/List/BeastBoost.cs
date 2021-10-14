using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastBoost : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.BeastBoost; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new BeastBoost(); }
    public override string Description()
    {
        return "The Pokémon boosts its most proficient stat each time it knocks out a Pokémon.";
    }
    public override StatBoost BoostStatUponKO(Pokemon attackingPokemon)
    {
        int highestStat = attackingPokemon.baseStats[StatAttribute.Attack];

        for (int i = (int)StatAttribute.Defense; i <= (int)StatAttribute.Speed; i++)
        {
            StatAttribute statAttribute = (StatAttribute)i;
            if (attackingPokemon.baseStats[statAttribute] > highestStat)
            {
                highestStat = attackingPokemon.baseStats[statAttribute];
            }
        }

        for (int i = (int)StatAttribute.Attack; i <= (int)StatAttribute.Speed; i++)
        {
            StatAttribute statAttribute = (StatAttribute)i;
            if (highestStat == attackingPokemon.baseStats[statAttribute])
            {
                if (attackingPokemon.statBoosts[statAttribute] < 6)
                {
                    return new StatBoost(statAttribute,1);
                }
                else
                {
                    return null;
                }
            }
        }

        return base.BoostStatUponKO(attackingPokemon);
    }
}
