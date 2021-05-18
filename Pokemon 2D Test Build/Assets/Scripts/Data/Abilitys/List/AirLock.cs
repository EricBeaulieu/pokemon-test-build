using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLock : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.AirLock; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new AirLock(); }
    public override string Description()
    {
        return "Eliminates the effects of weather.";
    }
    public override bool NegatesWeatherEffects()
    {
        return true;
    }
}
