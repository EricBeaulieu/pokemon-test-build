using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insomnia : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Insomnia; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Insomnia(); }
    public override string Description()
    {
        return "The Pokémon is suffering from insomnia and cannot fall asleep.";
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
        return $"{pokemon.currentName} can't sleep due to Insomnia";
    }
}
