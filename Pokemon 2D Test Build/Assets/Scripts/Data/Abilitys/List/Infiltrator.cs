using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infiltrator : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Infiltrator; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Infiltrator(); }
    public override string Description()
    {
        return "Passes through the opposing Pokémon's barrier, substitute, and the like and strikes.";
    }
    public override bool CutsThroughProtections()
    {
        return true;
    }

}