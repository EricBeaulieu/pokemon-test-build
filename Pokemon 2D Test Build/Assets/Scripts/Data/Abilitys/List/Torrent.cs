using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torrent : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Torrent; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Torrent(); }
    public override string Description()
    {
        return "Powers up Water-type moves when the Pokémon's HP is low.";
    }
    public override float BoostACertainTypeInAPinch(Pokemon attackingPokemon, ElementType attackType)
    {
        if (attackType != ElementType.Water)
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
