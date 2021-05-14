using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWeb : EntryHazardBase
{
    public override EntryHazardID Id { get { return EntryHazardID.StickyWeb; } }
    public override EntryHazardBase ReturnDerivedClassAsNew() { return new StickyWeb(); }
    protected override int _maxLayers() { return 1; }
    public override string StartMessage(BattleUnit battleUnit)
    {
        string message = "A sticky web has been laid out all around the ";

        if (battleUnit.isPlayerPokemon)
        {
            message += "Player's";
        }
        else
        {
            message += "foes's";
        }

        message += " team!";
        layers++;
        return message;
    }
    public override void OnEntry(Pokemon pokemon)
    {
        if (pokemon.pokemonBase.IsType(ElementType.Flying))
        {
            return;
        }

        pokemon.statusChanges.Enqueue($"{pokemon.currentName} was caught in the sticky web");
    }
    public override StatBoost OnEntryLowerStat(Pokemon pokemon)
    {
        if (pokemon.pokemonBase.IsType(ElementType.Flying))
        {
            return new StatBoost() { stat = StatAttribute.Speed, boost = 0 }; ;
        }

        return new StatBoost() { stat = StatAttribute.Speed, boost = -1 };
    }
}
