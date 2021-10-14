using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerPoint : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.AngerPoint; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new AngerPoint(); }
    public override string Description()
    {
        return "The Pokémon is angered when it takes a critical hit, and that maxes its Attack stat.";
    }
    public override StatBoost MaxOutStatUponCriticalHit(Pokemon defendingPokemon)
    {
        if (defendingPokemon.statBoosts[StatAttribute.Attack] < 6)
        {
            return new StatBoost(StatAttribute.Attack,12);
        }
        return base.MaxOutStatUponCriticalHit(defendingPokemon);
    }
}
