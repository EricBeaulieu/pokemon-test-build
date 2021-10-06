using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGift : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.FlowerGift; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new FlowerGift(); }
    public override string Description()
    {
        return "Boosts the Attack and Sp. Def stats of itself and allies when it is sunny.";
    }
    public override float AlterStat(WeatherEffectID iD, StatAttribute statAffected)
    {
        if (iD == WeatherEffectID.Sunshine && (statAffected == StatAttribute.Attack || statAffected == StatAttribute.SpecialDefense))
        {
            return 1.5f;
        }
        return base.AlterStat(iD, statAffected);
    }
}
