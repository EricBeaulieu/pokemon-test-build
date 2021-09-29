using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limber : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Limber; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Limber(); }
    public override string Description()
    {
        return "Its limber body protects the Pokémon from paralysis.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD, WeatherEffectID weather)
    {
        if (iD == ConditionID.Paralyzed)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD,weather);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s {Name} protects it from paralysis";
    }
}
