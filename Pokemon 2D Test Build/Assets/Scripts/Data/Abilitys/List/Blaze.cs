using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaze : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Blaze; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Blaze(); }
    public override string Description()
    {
        return "Powers up Fire-type moves when the Pokémon's HP is low.";
    }
    public override float BoostACertainTypeInAPinch(Pokemon attackingPokemon, ElementType attackType)
    {
        if (attackType != ElementType.Fire)
        {
            return base.BoostACertainTypeInAPinch(attackingPokemon, attackType);
        }

        if (attackingPokemon.currentHitPoints / attackingPokemon.maxHitPoints <= HpRequiredToActivatePinch)
        {
            return 1.5f;
        }
        return base.BoostACertainTypeInAPinch(attackingPokemon, attackType);
    }
}
