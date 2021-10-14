using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leek : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.Leek; } }
    public override float AlterStat(Pokemon holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.CriticalHitRatio && holder.pokemonBase.GetPokedexName() == "Farfetch'd" || holder.pokemonBase.GetPokedexName() == "Sirfetch'd")
        {
            return 2;
        }
        return base.AlterStat(holder, statAffected);
    }
}
