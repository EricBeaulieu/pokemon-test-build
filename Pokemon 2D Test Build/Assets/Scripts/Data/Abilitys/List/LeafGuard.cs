using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafGuard : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.LeafGuard; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new LeafGuard(); }
    public override string Description()
    {
        return "Prevents status conditions in harsh sunlight.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD, WeatherEffectID weather)
    {
        if (weather == WeatherEffectID.Sunshine)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD, weather);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s {Name} protects it from status conditions";
    }
}
