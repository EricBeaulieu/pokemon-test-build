using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyDutyBoots : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.HeavyDutyBoots; } }
    public override bool PreventsEffectsOfEntryHazards()
    {
        return true;
    }
}
