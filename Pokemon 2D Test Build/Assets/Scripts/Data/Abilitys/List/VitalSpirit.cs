using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalSpirit : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.VitalSpirit; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new VitalSpirit(); }
    public override string Description()
    {
        return "The Pokémon is full of vitality, and that prevents it from falling asleep.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD, WeatherEffectID weather)
    {
        if (iD == ConditionID.Sleep)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD,weather);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName} can't sleep due to Vital Spirit";
    }
}
