using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheri : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Cheri; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Cheri(); }

}