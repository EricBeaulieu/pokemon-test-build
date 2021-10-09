using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.Snowball; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Snowball(); }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost() { stat = StatAttribute.Attack, boost = 1 }
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(ElementType attackType, bool superEffective)
    {
        if (attackType == ElementType.Ice)
        {
            RemoveItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType,superEffective);
    }
}
