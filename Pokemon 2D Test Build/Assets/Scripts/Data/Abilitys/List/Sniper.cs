using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Sniper; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Sniper(); }
    public override string Description()
    {
        return "Powers up moves if they become critical hits when attacking.";
    }
    public override float AltersCriticalHitDamage()
    {
        return 2.25f;
    }
}
