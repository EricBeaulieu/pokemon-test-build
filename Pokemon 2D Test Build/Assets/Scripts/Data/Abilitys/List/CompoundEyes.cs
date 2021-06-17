using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundEyes : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.CompoundEyes; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new CompoundEyes(); }
    public override string Description()
    {
        return "The Pokémon's compound eyes boost its accuracy.";
    }
    public override float AlterStat(WeatherEffectID iD, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.Accuracy)
        {
            return 1.3f;
        }
        return base.AlterStat(iD, statAffected);
    }
}
