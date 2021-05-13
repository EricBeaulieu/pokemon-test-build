using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Poison; } }
    public override string StartMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} has been poisoned";
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
        int damage = Mathf.FloorToInt(pokemon.maxHitPoints / 8);

        if (damage <= 0)
        {
            damage = 1;
        }

        pokemon.UpdateHP(damage);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by poison");
    }
}
