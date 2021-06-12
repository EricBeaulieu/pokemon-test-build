using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowCloak : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SnowCloak; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SnowCloak(); }
    public override string Description()
    {
        return "Boosts evasiveness in a hailstorm.";
    }
    public override float AlterStatDuringWeatherEffect(WeatherEffectID iD, StatAttribute statAffected)
    {
        if (iD == WeatherEffectID.Hail && statAffected == StatAttribute.Evasion)
        {
            return 1.2f;
        }
        return base.AlterStatDuringWeatherEffect(iD, statAffected);
    }
}
