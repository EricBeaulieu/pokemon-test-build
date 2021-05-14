using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicPoison : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.ToxicPoison; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new ToxicPoison(); }
    public override string StartMessage(Pokemon pokemon)
    {
        StatusTime = 0;
        return $"{pokemon.currentName} has been badly poisoned";
    }
    public override bool HasCondition(ConditionID conditionID)
    {
        return (conditionID == ConditionID.Poison || conditionID == ConditionID.ToxicPoison);
    }
    public override string HasConditionMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} is already poisoned";
    }
    public override void OnEndTurn(Pokemon pokemon)
    {
        StatusTime++;
        int damage = pokemon.maxHitPoints / (16 / StatusTime);

        if (damage <= 0)
        {
            damage = 1;
        }

        pokemon.UpdateHP(damage);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by poison");
    }
}
