using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eviolite : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.Eviolite; } }
    public override float AlterStat(Pokemon holder, StatAttribute statAffected)
    {
        if(holder.pokemonBase.Evolutions.Count > 0)
        {
            if (statAffected == StatAttribute.Defense || statAffected == StatAttribute.SpecialDefense)
            {
                return 1.5f;
            }
        }
        return base.AlterStat(holder, statAffected);
    }
}
