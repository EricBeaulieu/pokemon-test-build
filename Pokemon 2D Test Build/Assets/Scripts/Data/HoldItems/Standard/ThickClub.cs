using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThickClub : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.ThickClub; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ThickClub(); }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if (Holder.pokemonBase.GetPokedexName() == "Cubone" || Holder.pokemonBase.GetPokedexName() == "Marowak" && statAffected == StatAttribute.Attack)
        {
            return 2f;
        }
        return base.AlterStat(Holder, statAffected);
    }
}
