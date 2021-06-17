using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiftSwim : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SwiftSwim; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SwiftSwim(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Speed stat in rain.";
    }
    public override float AlterStat(WeatherEffectID iD, StatAttribute statAffected)
    {
        if (iD == WeatherEffectID.Rain && statAffected == StatAttribute.Speed)
        {
            return 2;
        }
        return base.AlterStat(iD, statAffected);
    }
}
