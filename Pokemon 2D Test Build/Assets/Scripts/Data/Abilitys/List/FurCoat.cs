using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurCoat : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.FurCoat; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new FurCoat(); }
    public override string Description()
    {
        return "Halves the damage from physical moves.";
    }
    public override float AlterStat(WeatherEffectID iD, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.Defense)
        {
            return 2;
        }
        return base.AlterStat(iD, statAffected);
    }
}
