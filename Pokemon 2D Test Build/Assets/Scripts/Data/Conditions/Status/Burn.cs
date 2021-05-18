using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Burn; } }
    public override ConditionBase ReturnDerivedClassAsNew(){ return new Burn();}
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{pokemon.currentName} has been burned";
    }
    public override string HasConditionMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} is already burnt";
    }
    public override float StatEffectedByCondition(StatAttribute statAttribute)
    {
        if (statAttribute == StatAttribute.Attack)
        {
            return 0.5f;
        }
        return 1;
    }
    public override void OnEndTurn(Pokemon pokemon)
    {
        int damage = pokemon.maxHitPoints / 16;

        if (damage <= 0)
        {
            damage = 1;
        }

        pokemon.UpdateHPDamage(damage);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by burn");
    }
}
