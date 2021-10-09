using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepSeaScale : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.DeepSeaScale; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new DeepSeaScale(); }
    public override float AlterStat(Pokemon holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.SpecialDefense && holder.pokemonBase.GetPokedexName() == "Clamperl")
        {
            return 2f;
        }
        return base.AlterStat(holder, statAffected);
    }
}
