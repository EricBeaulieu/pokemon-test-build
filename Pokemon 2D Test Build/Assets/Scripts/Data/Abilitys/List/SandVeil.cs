using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandVeil : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SandVeil; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SandVeil(); }
    public override string Description()
    {
        return "Boosts the Pokémon's evasiveness in a sandstorm.";
    }
    public override float AlterStatDuringWeatherEffect(WeatherEffectID iD, StatAttribute statAffected)
    {
        if (iD == WeatherEffectID.Sandstorm && statAffected == StatAttribute.Evasion)
        {
            return 1.2f;
        }
        return base.AlterStatDuringWeatherEffect(iD, statAffected);
    }
}
