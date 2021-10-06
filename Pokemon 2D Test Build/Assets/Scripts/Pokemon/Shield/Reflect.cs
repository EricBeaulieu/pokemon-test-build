using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : ShieldBase
{
    public Reflect(ItemBase item)
    {
        shield = ShieldType.Reflect;
        NewShield(item);
    }

    public override int ProtectedStat(StatAttribute stat)
    {
        if(stat == StatAttribute.Defense)
        {
            return 2;
        }
        return 1;
    }

    public override string StartMessage(bool isPlayer)
    {
        if (isPlayer == true)
        {
            return "Reflect made your team stronger against physical moves";
        }
        return "Reflect made your foe's team stronger against physical moves";
    }

    public override string EndMessage(bool isPlayer)
    {
        if (isPlayer == true)
        {
            return "Your team's Reflect wore off!";
        }
        return "Your foe's Reflect wore off!";
    }
}
