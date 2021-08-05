using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugePower : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.HugePower; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new HugePower(); }
    public override string Description()
    {
        return "Doubles the Pokémon's Attack stat.";
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
