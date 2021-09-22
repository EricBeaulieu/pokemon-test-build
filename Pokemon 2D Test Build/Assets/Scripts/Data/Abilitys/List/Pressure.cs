using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressure : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Pressure; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Pressure(); }
    public override string Description()
    {
        return "By putting pressure on the opposing Pokémon, it raises their PP usage.";
    }
    public override bool ReducesPowerPointsBy2()
    {
        return true;
    }
}
