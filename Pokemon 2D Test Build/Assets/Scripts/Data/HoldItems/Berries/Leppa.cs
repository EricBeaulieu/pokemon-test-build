using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leppa : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Leppa; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Leppa(); }

}