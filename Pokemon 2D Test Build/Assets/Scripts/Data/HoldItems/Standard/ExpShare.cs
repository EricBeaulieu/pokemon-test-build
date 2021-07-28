using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpShare : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ExpShare; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ExpShare(); }
    public override bool ExperienceShared()
    {
        return true;
    }
}
