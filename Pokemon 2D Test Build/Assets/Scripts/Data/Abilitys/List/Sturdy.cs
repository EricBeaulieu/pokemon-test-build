using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sturdy : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Sturdy; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Sturdy(); }
    public override string Description()
    {
        return "It cannot be knocked out with one hit. One-hit KO moves cannot knock it out, either.";
    }
    public override bool PreventsOneHitKO(Pokemon defendingPokemon, int damage)
    {
        if(damage >= defendingPokemon.maxHitPoints && defendingPokemon.currentHitPoints == defendingPokemon.maxHitPoints)
        {
            return true;
        }
        return base.PreventsOneHitKO(defendingPokemon, damage);
    }
}
