using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyPunch : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LuckyPunch; } }
    public override float AlterStat(Pokemon holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.CriticalHitRatio && holder.pokemonBase.GetPokedexName() == "Chansey")
        {
            return 2;
        }
        return base.AlterStat(holder, statAffected);
    }
}
