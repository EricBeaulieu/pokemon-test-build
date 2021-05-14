using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantEscape : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.CantEscape; } }
    public override bool PreventsEscape() { return true; }
}
