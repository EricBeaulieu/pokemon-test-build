using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overgrown : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Overgrown; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Overgrown(); }
    public override string Description()
    {
        return "Powers up Grass-type moves when the Pokémon's HP is low.";
    }
    public override float BoostACertainTypeInAPinch(Pokemon attackingPokemon, ElementType attackType)
    {
        if (attackType != ElementType.Grass)
        {
            return base.BoostACertainTypeInAPinch(attackingPokemon, attackType);
        }

        if (((float)attackingPokemon.currentHitPoints / (float)attackingPokemon.maxHitPoints) <= HpRequiredToActivatePinch)
        {
            return 1.5f;
        }
        return base.BoostACertainTypeInAPinch(attackingPokemon, attackType);
    }
}
