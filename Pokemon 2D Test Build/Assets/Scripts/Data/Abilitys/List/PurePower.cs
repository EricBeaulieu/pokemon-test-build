using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurePower : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.PurePower; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new PurePower(); }
    public override string Description()
    {
        return "Using its pure power, the Pokémon doubles its Attack stat.";
    }
    public override float AlterStat(WeatherEffectID iD, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.Attack)
        {
            return 2;
        }
        return base.AlterStat(iD, statAffected);
    }
}
