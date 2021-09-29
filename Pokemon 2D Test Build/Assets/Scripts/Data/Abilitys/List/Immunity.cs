using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immunity : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Immunity; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Immunity(); }
    public override string Description()
    {
        return "The immune system of the Pokémon prevents it from getting poisoned.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD, WeatherEffectID weather)
    {
        if (iD == ConditionID.Poison || iD == ConditionID.ToxicPoison)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD,weather);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s {Name} prevents poisoning";
    }
}
