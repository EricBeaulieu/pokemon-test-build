using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrappy : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Scrappy; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Scrappy(); }
    public override string Description()
    {
        return "The Pokémon can hit Ghost-type Pokémon with Normal- and Fighting-type moves.";
    }
    public override bool DamageDealingMovesCutThroughNaturalImmunity(Pokemon defendingPokemon, ElementType attackType)
    {
        if (defendingPokemon.pokemonBase.IsType(ElementType.Ghost))
        {
            if(attackType == ElementType.Fighting||attackType == ElementType.Normal)
            {
                return true;
            }
        }
        return base.DamageDealingMovesCutThroughNaturalImmunity(defendingPokemon,attackType);
    }
}
