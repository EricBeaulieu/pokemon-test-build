using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScreen : ShieldBase
{
    public LightScreen(ItemBase item)
    {
        shield = ShieldType.LightScreen;
        NewShield(item);
    }

    public override int ProtectedStat(StatAttribute stat)
    {
        if (stat == StatAttribute.SpecialDefense)
        {
            return 2;
        }
        return 1;
    }

    public override string StartMessage(bool isPlayer)
    {
        if (isPlayer == true)
        {
            return "Light Screen made your team stronger against special moves";
        }
        return "Light Screen made your foe's team stronger against special moves";
    }

    public override string EndMessage(bool isPlayer)
    {
        if (isPlayer == true)
        {
            return "Your team's Light Screen wore off!";
        }
        return "Your foe's Light Screen wore off!";
    }
}
