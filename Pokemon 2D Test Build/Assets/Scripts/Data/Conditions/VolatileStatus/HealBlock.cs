using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBlock : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.HealBlock; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new HealBlock(); }
    public override string StartMessage(Pokemon pokemon)
    {
        StatusTime = 5;
        return $"{pokemon.currentName} was prevented from healing";
    }
    public override void OnEndTurn(Pokemon pokemon)
    {
        StatusTime--;
        if (StatusTime <= 0)
        {
            pokemon.CureVolatileStatus(Id);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName}'s Heal Block ended");
        }
    }
    public override bool PreventsHealing()
    {
        return true;
    }
}
