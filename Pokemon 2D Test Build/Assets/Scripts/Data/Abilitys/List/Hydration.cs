using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydration : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Hydration; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Hydration(); }
    public override string Description()
    {
        return "Heals status conditions if it's raining.";
    }
    public override bool CuresStatusAtTurnEnd(Pokemon defendingPokemon,WeatherEffectID weather)
    {
        if(defendingPokemon.status == null)
        {
            return false;
        }

        if(weather == WeatherEffectID.Rain)
        {
            return true;
        }

        return base.CuresStatusAtTurnEnd(defendingPokemon, weather);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        string abilityMessage = $"{pokemon.currentName} was cured of it's ";
        switch (pokemon.status.Id)
        {
            case ConditionID.Burn:
                abilityMessage += "burning!";
                break;
            case ConditionID.Sleep:
                abilityMessage += "sleep!";
                break;
            case ConditionID.Paralyzed:
                abilityMessage += "paralysis!";
                break;
            case ConditionID.Frozen:
                abilityMessage += "freezing!";
                break;
            default://poison/toxic
                abilityMessage += "poisoning!";
                break;
        }
        pokemon.CureStatus();
        return $"{abilityMessage}";
    }
}