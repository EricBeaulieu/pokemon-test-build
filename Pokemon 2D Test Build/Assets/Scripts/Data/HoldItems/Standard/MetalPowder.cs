using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPowder : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.LightBall; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LightBall(); }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if (Holder.pokemonBase.GetPokedexName() == "Ditto" && statAffected == StatAttribute.Defense || statAffected == StatAttribute.SpecialDefense)
        {
            return 1.5f;
        }
        return base.AlterStat(Holder, statAffected);
    }
}
