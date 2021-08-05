using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyEgg : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.LuckyEgg; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LuckyEgg(); }
    public override float ExperienceModifier()
    {
        return 1.5f;
    }
}
