using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursed : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Cursed; } }
    public override string HasConditionMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} is already Cursed";
    }
    public override void OnEndTurn(Pokemon pokemon)
    {
        pokemon.UpdateHP(Mathf.CeilToInt((float)pokemon.maxHitPoints / 4f));
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} is afflicted by the curse");
    }
}
