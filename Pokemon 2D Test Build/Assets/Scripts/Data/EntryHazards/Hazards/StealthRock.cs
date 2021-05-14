using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthRock : EntryHazardBase
{
    public override EntryHazardID Id { get { return EntryHazardID.StealthRock; } }
    protected override int _maxLayers() { return 1; }
    public override string StartMessage(BattleUnit battleUnit)
    {
        string message = "Pointed Stones are floating in the air of your ";

        if (battleUnit.isPlayerPokemon == false)
        {
            message += "foes's ";
        }

        message += "team!";

        layers++;
        return message;
    }
    public override void OnEntry(Pokemon pokemon)
    {
        float damageEffectiveness = DamageModifiers.TypeChartEffectiveness(pokemon.pokemonBase, ElementType.Rock);

        int damage = Mathf.FloorToInt(pokemon.maxHitPoints / (8 / damageEffectiveness));

        if (damage <= 0)
        {
            damage = 1;
        }

        pokemon.UpdateHP(damage);
        pokemon.statusChanges.Enqueue($"Pointed Stones Dug into {pokemon.currentName}");
    }
}
