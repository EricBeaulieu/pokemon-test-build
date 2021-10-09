using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulDew : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.SoulDew; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new SoulDew(); }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if (Holder.pokemonBase.GetPokedexName() == "Latios" || Holder.pokemonBase.GetPokedexName() == "Latias" && statAffected == StatAttribute.SpecialAttack || statAffected == StatAttribute.SpecialDefense)
        {
            return 1.5f;
        }
        return base.AlterStat(Holder, statAffected);
    }
}
