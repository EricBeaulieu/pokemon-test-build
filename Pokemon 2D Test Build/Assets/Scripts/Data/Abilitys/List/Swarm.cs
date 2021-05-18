using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Swarm; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Swarm(); }
    public override string Description()
    {
        return "Powers up Bug-type moves when the Pokémon's HP is low.";
    }
    public override float BoostACertainTypeInAPinch(Pokemon attackingPokemon, ElementType attackType)
    {
        if (attackType != ElementType.Bug)
        {
            return base.BoostACertainTypeInAPinch(attackingPokemon, attackType);
        }

        if (attackingPokemon.currentHitPoints / attackingPokemon.maxHitPoints <= 1 / 3)
        {
            return 1.5f;
        }
        return base.BoostACertainTypeInAPinch(attackingPokemon, attackType);
    }
}
