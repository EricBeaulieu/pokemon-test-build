using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBall : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.SmokeBall; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new SmokeBall(); }
    public override bool FleeWithoutFail()
    {
        return true;
    }
}
