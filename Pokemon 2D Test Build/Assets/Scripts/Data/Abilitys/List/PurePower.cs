using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurePower : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.PurePower; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new PurePower(); }
    public override string Description()
    {
        return "Using its pure power, the Pokémon doubles its Attack stat.";
    }
    public override int DoublesAStat(StatAttribute stat)
    {
        if (stat == StatAttribute.Attack)
        {
            return 2;
        }
        return base.DoublesAStat(stat);
    }
}
