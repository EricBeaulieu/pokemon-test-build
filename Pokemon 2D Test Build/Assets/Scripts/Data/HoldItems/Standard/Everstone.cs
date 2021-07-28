using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Everstone : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ElectricGem; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ElectricGem(); }
    public override bool PreventsPokemonFromEvolving()
    {
        return true;
    }
}
