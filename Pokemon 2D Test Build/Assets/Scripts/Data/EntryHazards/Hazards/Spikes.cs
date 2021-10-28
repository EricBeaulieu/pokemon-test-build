using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : EntryHazardBase
{
    public override EntryHazardID Id { get { return EntryHazardID.Spikes; } }
    public override EntryHazardBase ReturnDerivedClassAsNew() { return new Spikes(); }
    protected override int maxLayers() { return 3; }
    public override string StartMessage(BattleUnit battleUnit)
    {
        string message = "Spikes were scattered all around the feet of the ";

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
    public override void OnEntry(BattleUnit defendingUnit)
    {
        if (defendingUnit.IsGrounded() == true)
        {
            return;
        }

        int damage = defendingUnit.pokemon.maxHitPoints / (10 - (layers * 2));

        if (damage <= 0)
        {
            damage = 1;
        }

        defendingUnit.pokemon.UpdateHPDamage(damage);
        defendingUnit.pokemon.statusChanges.Enqueue($"{defendingUnit.pokemon.currentName} is hurt Spikes");
    }
}
