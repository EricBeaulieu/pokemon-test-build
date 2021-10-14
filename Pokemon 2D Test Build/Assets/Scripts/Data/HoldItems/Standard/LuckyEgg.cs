using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyEgg : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LuckyEgg; } }
    public override float ExperienceModifier()
    {
        return 1.5f;
    }
}
