using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBattery : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.CellBattery; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new CellBattery(); }
    public override StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType)
    {
        if (attackType == ElementType.Electric)
        {
            RemoveItem();
            return new StatBoost() { stat = StatAttribute.Attack, boost = 1 };
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
