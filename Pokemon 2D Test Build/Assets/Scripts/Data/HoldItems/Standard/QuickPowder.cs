using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickPowder : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.QuickPowder; } }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if (Holder.pokemonBase.GetPokedexName() == "Ditto" && statAffected == StatAttribute.Speed)
        {
            return 2f;
        }
        return base.AlterStat(Holder, statAffected);
    }
}
