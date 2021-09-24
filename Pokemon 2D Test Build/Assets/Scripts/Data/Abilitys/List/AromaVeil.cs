using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AromaVeil : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.AromaVeil; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new AromaVeil(); }
    public override string Description()
    {
        return "Protects itself and its allies from attacks that limit their move choices.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD, WeatherEffectID weather)
    {// Taunt, Torment, Encore, Disable and Cursed Body, Heal Block, and infatuation.
        if (iD == ConditionID.Encore || iD == ConditionID.HealBlock || iD == ConditionID.Infatuation)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD, weather);
    }
}
