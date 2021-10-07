using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidOoze : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.LiquidOoze; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new LiquidOoze(); }
    public override string Description()
    {
        return "Oozed liquid has strong stench, which damages attackers using any draining move.";
    }
    public override bool DamagesOpponentUponAbsorbingHP()
    {
        return true;
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"The {pokemon.currentName} sucked up the liquid ooze!";
    }
}