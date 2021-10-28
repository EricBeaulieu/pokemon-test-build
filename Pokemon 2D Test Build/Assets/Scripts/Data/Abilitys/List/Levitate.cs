using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Levitate; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Levitate(); }
    public override string Description()
    {
        return "By floating in the air, the Pokémon receives full immunity to all Ground-type moves.";
    }
    public override bool Levitates()
    {
        return true;
    }
}
