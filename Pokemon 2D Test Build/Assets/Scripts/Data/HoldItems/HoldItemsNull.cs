using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItemsNull : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.NA; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new HoldItemsNull(); }
}
