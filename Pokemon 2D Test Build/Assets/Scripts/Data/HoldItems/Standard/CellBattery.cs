using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBattery : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.CellBattery; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new CellBattery(); }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost() { stat = StatAttribute.Attack, boost = 1 }
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(ElementType attackType, bool superEffective)
    {
        if (attackType == ElementType.Electric)
        {
            RemoveItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType,superEffective);
    }
}
