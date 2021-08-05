using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyDutyBoots : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.HeavyDutyBoots; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new HeavyDutyBoots(); }
    public override bool PreventsEffectsOfEntryHazards()
    {
        return true;
    }
}
