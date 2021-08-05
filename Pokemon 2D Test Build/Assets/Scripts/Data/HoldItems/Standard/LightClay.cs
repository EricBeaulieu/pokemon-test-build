using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightClay : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.LightClay; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LightClay(); }
    public override int ReflectLightScreenDuration(MoveBase move)
    {
        return base.ReflectLightScreenDuration(move);
    }
}
