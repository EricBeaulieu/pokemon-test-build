using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechSeed : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.LeechSeed; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new LeechSeed(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{pokemon.currentName} was seeded";
    }
    public override void OnEndTurn(Pokemon pokemon)
    {
        int damage = Mathf.FloorToInt(pokemon.maxHitPoints / 8);
        damage = Mathf.Clamp(damage, 1, pokemon.currentHitPoints);
        HealthStolen = damage;

        pokemon.UpdateHPDamage(damage);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} health is sapped by leech seed");
    }
    public int HealthStolen { get; private set; }
}
