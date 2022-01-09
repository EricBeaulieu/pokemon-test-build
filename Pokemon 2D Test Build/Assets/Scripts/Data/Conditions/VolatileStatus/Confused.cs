using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confused : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Confused; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Confused(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        StatusTime = Random.Range(2, 6);
        return $"{pokemon.currentName} has been confused";
    }
    public override string HasConditionMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} is already Confused";
    }
    public override bool OnBeforeMove(Pokemon source, Pokemon target = null)
    {
        if (StatusTime <= 0)
        {
            source.CureVolatileStatus(Id);
            source.statusChanges.Enqueue($"{source.currentName} is no longer confused");
            return true;
        }

        StatusTime--;
        source.statusChanges.Enqueue($"{source.currentName} is confused");

        if (Random.value > 0.5f)
        {
            return true;
        }
        
        int damage = Mathf.FloorToInt(((((2 * source.currentLevel) / 5) + 2) * 40 * source.attack / source.defense / 50) + 2);

        if (damage <= 0)
        {
            damage = 1;
        }

        source.UpdateHPDamage(damage);
        source.statusChanges.Enqueue($"{source.currentName} hurt itself in its confusion");
        return false;
    }
    public override bool PreAttackAnimation()
    {
        return true;
    }
}
