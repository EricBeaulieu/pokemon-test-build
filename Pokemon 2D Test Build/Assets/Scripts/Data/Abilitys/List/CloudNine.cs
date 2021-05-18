using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudNine : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.CloudNine; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new CloudNine(); }
    public override string Description()
    {
        return "Eliminates the effects of weather.";
    }
    public override bool NegatesWeatherEffects()
    {
        return true;
    }
}
