using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aftermath : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Aftermath; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Aftermath(); }
    public override string Description()
    {
        return "Damages the attacker if it contacts the Pokémon with a finishing hit.";
    }
    public override bool DamagesAttackerUponFinishingHit(Pokemon defendingPokemon, Pokemon attackingPokemon, MoveBase currentAttack)
    {
        if(defendingPokemon.currentHitPoints > 0)
        {
            return false;
        }

        if (attackingPokemon.ability.Id == AbilityID.Damp || currentAttack.MoveType != MoveType.Physical)
        {
            return false;
        }

        int damage = attackingPokemon.maxHitPoints / 4;

        if (damage <= 0)
        {
            damage = 1;
        }

        attackingPokemon.UpdateHPDamage(damage);
        attackingPokemon.statusChanges.Enqueue($"{attackingPokemon.currentName} is hurt");

        return true;
    }
}
