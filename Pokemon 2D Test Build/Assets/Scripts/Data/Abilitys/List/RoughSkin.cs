using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoughSkin : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.RoughSkin; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new RoughSkin(); }
    public override string Description()
    {
        return "This Pokémon inflicts damage with its rough skin to the attacker on contact.";
    }
    public override bool DamagesAttackerUponHit(Pokemon defendingPokemon, Pokemon attackingPokemon, MoveBase currentAttack)
    {
        if (currentAttack.PhysicalContact != true)
        {
            return false;
        }

        int damage = attackingPokemon.maxHitPoints / 8;

        if (damage <= 0)
        {
            damage = 1;
        }

        attackingPokemon.UpdateHPDamage(damage);
        attackingPokemon.statusChanges.Enqueue($"{attackingPokemon.currentName} is hurt");

        return true;
    }
}