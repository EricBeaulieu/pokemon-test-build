using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingBand : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.BindingBand; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new BindingBand(); }
    public override bool BindingDamageIncreased() { return true; }
}
