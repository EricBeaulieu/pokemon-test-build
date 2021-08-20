using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Everstone : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.Everstone; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Everstone(); }
    public override bool PreventsPokemonFromEvolving()
    {
        return true;
    }
}