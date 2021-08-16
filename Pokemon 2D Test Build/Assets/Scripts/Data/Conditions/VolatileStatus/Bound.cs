using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Bound; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Bound(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        poweredUp = attackingPokemon.GetHoldItemEffects.BindingDamageIncreased();
        StatusTime = Random.Range(4, 6);
        if(attackingPokemon.GetHoldItemEffects.ExtendsBindToMaxPotential() == true)
        {
            StatusTime = 7;
        }
        return $"{pokemon.currentName} {GlobalTools.ReplaceUserWithPokemonName(boundMove.SecondaryEffects[0].SpecialStartMessage, attackingPokemon)}";
    }
    public override bool OnEndTurn(Pokemon pokemon)
    {
        StatusTime--;
        if(StatusTime <= 0)
        {
            pokemon.CureVolatileStatus(Id);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName} was freed from {boundMove.MoveName}!");
            return false;
        }

        int damage = Mathf.FloorToInt(pokemon.maxHitPoints / healthPercentage);

        if (damage <= 0)
        {
            damage = 1;
        }
        
        pokemon.UpdateHPDamage(damage);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by {boundMove.MoveName}!");
        return true;
    }
    public MoveBase SetBoundMove { set { boundMove = value; } }
    MoveBase boundMove = null;
    bool poweredUp;
    int healthPercentage
    {
        get
        {
            if (poweredUp == true)
            {
                return 6;
            }
            return 8;
        }
    }
}
