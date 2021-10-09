using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eviolite : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.Eviolite; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Eviolite(); }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        //if pokemon can evolve

        if(statAffected == StatAttribute.Defense || statAffected == StatAttribute.SpecialDefense)
        {
            return 1.5f;
        }

        return base.AlterStat(Holder, statAffected);
    }
}
