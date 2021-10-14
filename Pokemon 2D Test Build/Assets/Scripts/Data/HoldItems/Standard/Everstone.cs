using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Everstone : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.Everstone; } }
    public override bool PreventsPokemonFromEvolving()
    {
        return true;
    }
}
