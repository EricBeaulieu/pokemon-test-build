using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaRing : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.AquaRing; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new AquaRing(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{pokemon.currentName} surrounded itself in a veil of water!";
    }
    public override bool OnEndTurn(Pokemon pokemon)
    {
        if(pokemon.currentHitPoints == pokemon.maxHitPoints)
        {
            return false;
        }

        int hphealed = Mathf.FloorToInt(pokemon.maxHitPoints / 16);

        if (hphealed <= 0)
        {
            hphealed = 1;
        }

        pokemon.UpdateHPRestored(hphealed);
        pokemon.statusChanges.Enqueue($"A veil of water restored {pokemon.currentName}'s HP!");
        return true;
    }
}
