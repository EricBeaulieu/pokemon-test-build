using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessPolicy : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.WeaknessPolicy; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new WeaknessPolicy(); }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost() { stat = StatAttribute.Attack, boost = 2 },
        new StatBoost() { stat = StatAttribute.SpecialAttack, boost = 2 }
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(ElementType attackType, bool superEffective)
    {
        if (superEffective == true)
        {
            RemoveItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType, superEffective);
    }
}
