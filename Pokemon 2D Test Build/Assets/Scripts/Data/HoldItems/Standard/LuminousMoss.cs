using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuminousMoss : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LuminousMoss; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LuminousMoss(); }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost() { stat = StatAttribute.SpecialDefense, boost = 1 }
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(ElementType attackType, bool superEffective)
    {
        if (attackType == ElementType.Water)
        {
            RemoveItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType,superEffective);
    }
}
