using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGuard : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.NoGuard; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new NoGuard(); }
    public override string Description()
    {
        return "The Pokémon employs no-guard tactics to ensure incoming and outgoing attacks always land.";
    }
    public override bool IncomingAndOutgoingAttacksAlwaysLand()
    {
        return true;
    }
}
