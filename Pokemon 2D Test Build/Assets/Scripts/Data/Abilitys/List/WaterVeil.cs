using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVeil : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.WaterVeil; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new WaterVeil(); }
    public override string Description()
    {
        return "The Pokémon is covered with a water veil, which prevents the Pokémon from getting a burn.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD, WeatherEffectID weather)
    {
        if (iD == ConditionID.Burn)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD,weather);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s Water Veil prevents burning";
    }
}
