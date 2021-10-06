using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarlyBird : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.EarlyBird; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new EarlyBird(); }
    public override string Description()
    {
        return "The Pokémon awakens twice as fast as other Pokémon from sleep.";
    }
    public override bool HalfDurationOfSleep()
    {
        return true;
    }
}
