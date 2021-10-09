using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbBulb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.AbsorbBulb; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new AbsorbBulb(); }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost() { stat = StatAttribute.SpecialAttack, boost = 1 }
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(ElementType attackType, bool superEffective)
    {
        if(attackType == ElementType.Water)
        {
            RemoveItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType,superEffective);
    }
}
