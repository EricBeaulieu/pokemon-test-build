using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnTempo : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.OwnTempo; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new OwnTempo(); }
    public override string Description()
    {
        return "This Pokémon has its own tempo, and that prevents it from becoming confused.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD, WeatherEffectID weather)
    {
        if (iD == ConditionID.Confused)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD, weather);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s Own Tempo prevents Confusion";
    }
}
