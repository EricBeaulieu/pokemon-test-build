using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBall : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LightBall; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LightBall(); }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if(Holder.pokemonBase.GetPokedexName() == "Pikachu" && statAffected == StatAttribute.Attack || statAffected == StatAttribute.SpecialAttack)
        {
            return 2f;
        }
        return base.AlterStat(Holder, statAffected);
    }
}
