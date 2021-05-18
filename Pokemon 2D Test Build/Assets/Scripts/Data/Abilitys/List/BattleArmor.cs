using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArmor : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.BattleArmor; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new BattleArmor(); }
    public override string Description()
    {
        return "Hard armor protects the Pokémon from critical hits.";
    }
    public override bool PreventsCriticalHits()
    {
        return true;
    }
}
