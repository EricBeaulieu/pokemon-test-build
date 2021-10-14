using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpShare : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.ExpShare; } }
    public override bool ExperienceShared()
    {
        return true;
    }
}
