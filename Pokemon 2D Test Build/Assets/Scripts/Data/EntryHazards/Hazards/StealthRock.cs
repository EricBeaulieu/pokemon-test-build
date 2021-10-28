using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthRock : EntryHazardBase
{
    public override EntryHazardID Id { get { return EntryHazardID.StealthRock; } }
    public override EntryHazardBase ReturnDerivedClassAsNew() { return new StealthRock(); }
    protected override int maxLayers() { return 1; }
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
    public override void OnEntry(BattleUnit defendingUnit)
    {
        float damageEffectiveness = DamageModifiers.TypeChartEffectiveness(defendingUnit, ElementType.Rock);

        int damage = Mathf.FloorToInt(defendingUnit.pokemon.maxHitPoints / (8 / damageEffectiveness));

        if (damage <= 0)
        {
            damage = 1;
        }

        defendingUnit.pokemon.UpdateHPDamage(damage);
        defendingUnit.pokemon.statusChanges.Enqueue($"Pointed Stones Dug into {defendingUnit.pokemon.currentName}");
    }
}
