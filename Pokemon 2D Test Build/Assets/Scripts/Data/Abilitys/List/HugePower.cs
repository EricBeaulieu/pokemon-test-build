using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugePower : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.HugePower; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new HugePower(); }
    public override string Description()
    {
        return "Doubles the Pokémon's Attack stat.";
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
