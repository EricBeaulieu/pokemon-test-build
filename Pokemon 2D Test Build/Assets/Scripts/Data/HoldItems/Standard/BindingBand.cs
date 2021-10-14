using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingBand : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.BindingBand; } }
    public override bool BindingDamageIncreased() { return true; }
}
