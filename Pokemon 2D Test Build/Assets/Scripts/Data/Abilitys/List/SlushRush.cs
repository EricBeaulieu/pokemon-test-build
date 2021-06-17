using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlushRush : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SlushRush; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SlushRush(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Speed stat in a hailstorm.";
    }
    public override float AlterStat(WeatherEffectID iD, StatAttribute statAffected)
    {
        if (iD == WeatherEffectID.Hail && statAffected == StatAttribute.Speed)
        {
            return 2;
        }
        return base.AlterStat(iD, statAffected);
    }
}
