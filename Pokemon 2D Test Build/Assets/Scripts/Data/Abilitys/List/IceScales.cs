using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScales : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.IceScales; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new IceScales(); }
    public override string Description()
    {
        return "The Pok�mon is protected by ice scales, which halve the damage taken from special moves.";
    }
    public override int DoublesAStat(StatAttribute stat)
    {
        if (stat == StatAttribute.SpecialDefense)
        {
            return 2;
        }
        return base.DoublesAStat(stat);
    }
}