using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismArmor : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.PrismArmor; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new PrismArmor(); }
    public override string Description()
    {
        return "Reduces the power of supereffective attacks taken.";
    }
    public override float LowersDamageTakeSuperEffectiveMoves(float typeEffectiveness)
    {
        if (typeEffectiveness > 1)
        {
            return DamageLoweredPercentage;
        }
        return base.LowersDamageTakeSuperEffectiveMoves(typeEffectiveness);
    }
}
