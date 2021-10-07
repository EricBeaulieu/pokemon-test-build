using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBarbs : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.IronBarbs; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new IronBarbs(); }
    public override string Description()
    {
        return "Inflicts damage to the attacker on contact with iron barbs.";
    }
    public override bool DamagesAttackerUponHit(Pokemon defendingPokemon, Pokemon attackingPokemon, MoveBase currentAttack)
    {
        if(currentAttack.PhysicalContact != true)
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