using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepSeaTooth : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.DeepSeaTooth; } }
    public override float AlterStat(Pokemon holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.SpecialAttack && holder.pokemonBase.GetPokedexName() == "Clamperl")
        {
            return 2f;
        }
        return base.AlterStat(holder, statAffected);
    }
}
