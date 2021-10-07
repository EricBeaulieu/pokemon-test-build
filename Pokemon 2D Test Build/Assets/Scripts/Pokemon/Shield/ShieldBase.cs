using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShieldType { LightScreen, Reflect, AuroraVeil }

public abstract class ShieldBase
{
    public void NewShield(ItemBase item)
    {
        duration = standardDuration;

        if (item != null)
        {
            duration += item.HoldItemAffects().ShieldDurationBonus();
        }
    }

    public int duration { get; private set; }
    const int standardDuration = 5;
    protected ShieldType shield;
    public ShieldType GetShieldType { get { return shield; } }
    public abstract int ProtectedStat(StatAttribute stat);
    public void TurnEndReduction() { duration--; }
    public abstract string StartMessage(bool isPlayer);
    public abstract string EndMessage(bool isPlayer);
}
