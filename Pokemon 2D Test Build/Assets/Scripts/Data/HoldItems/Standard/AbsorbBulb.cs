using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbBulb : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.AbsorbBulb; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new AbsorbBulb(); }
    public override StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType)
    {
        if(attackType == ElementType.Water)
        {
            RemoveItem();
            return new StatBoost() { stat = StatAttribute.SpecialAttack, boost = 1 };
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
