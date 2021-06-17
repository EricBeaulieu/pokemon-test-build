using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chlorophyll : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Chlorophyll; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Chlorophyll(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Speed stat in sunshine.";
    }
    public override float AlterStat(WeatherEffectID iD, StatAttribute statAffected)
    {
        if (iD == WeatherEffectID.Sunshine && statAffected == StatAttribute.Speed)
        {
            return 2;
        }
        return base.AlterStat(iD,statAffected);
    }
}
