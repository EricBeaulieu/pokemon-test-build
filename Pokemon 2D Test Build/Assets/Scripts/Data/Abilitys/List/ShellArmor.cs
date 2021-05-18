using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellArmor : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.ShellArmor; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new ShellArmor(); }
    public override string Description()
    {
        return "A hard shell protects the Pokémon from critical hits.";
    }
    public override bool PreventsCriticalHits()
    {
        return true;
    }
}
