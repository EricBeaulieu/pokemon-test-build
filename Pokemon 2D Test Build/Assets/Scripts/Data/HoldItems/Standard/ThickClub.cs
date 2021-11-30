using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThickClub : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.ThickClub; } }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if (Holder.pokemonBase.GetPokedexName() == "Cubone" && statAffected == StatAttribute.Attack || Holder.pokemonBase.GetPokedexName() == "Marowak" && statAffected == StatAttribute.Attack)
        {
            return 2f;
        }
        return base.AlterStat(Holder, statAffected);
    }
}
