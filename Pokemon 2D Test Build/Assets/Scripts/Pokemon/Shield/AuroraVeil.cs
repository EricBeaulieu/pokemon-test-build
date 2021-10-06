using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuroraVeil : ShieldBase
{
    public AuroraVeil(ItemBase item)
    {
        shield = ShieldType.AuroraVeil;
        NewShield(item);
    }

    public override int ProtectedStat(StatAttribute stat)
    {
        if (stat == StatAttribute.Defense || stat == StatAttribute.SpecialDefense)
        {
            return 2;
        }
        return 1;
    }

    public override string StartMessage(bool isPlayer)
    {
        if (isPlayer == true)
        {
            return "Aurora Veil made your team stronger against physical and special moves";
        }
        return "Aurora Veil made your foe's team stronger against physical and special moves";
    }

    public override string EndMessage(bool isPlayer)
    {
        if (isPlayer == true)
        {
            return "Your team's Aurora Veil wore off!";
        }
        return "Your foe's Aurora Veil wore off!";
    }
}
