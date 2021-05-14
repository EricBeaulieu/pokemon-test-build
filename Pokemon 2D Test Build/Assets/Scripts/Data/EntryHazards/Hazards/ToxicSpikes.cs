using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSpikes : EntryHazardBase
{
    public override EntryHazardID Id { get { return EntryHazardID.ToxicSpikes; } }
    public override EntryHazardBase ReturnDerivedClassAsNew() { return new ToxicSpikes(); }
    protected override int _maxLayers() { return 2; }
    public override string StartMessage(BattleUnit battleUnit)
    {
        string message = "Poisonous Spikes were scattered all around the feet of the ";

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
        if (pokemon.pokemonBase.IsType(ElementType.Flying) || pokemon.status != null)
        {
            return;
        }

        if (pokemon.pokemonBase.IsType(ElementType.Poison) || pokemon.pokemonBase.IsType(ElementType.Steel))
        {
            pokemon.statusChanges.Enqueue($"{pokemon.currentName} is uneffected by the toxic spikes");
            return;
        }

        if (layers == 0)
        {
            return;
        }
        else if (layers == 1)
        {
            pokemon.SetStatus(ConditionID.Poison, false);
        }
        else
        {
            pokemon.SetStatus(ConditionID.ToxicPoison, false);
        }
    }
}
