using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampRock : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.DampRock; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new DampRock(); }
    protected override WeatherEffectID WeatherEffectAmplified()
    {
        return WeatherEffectID.Rain;
    }

}
