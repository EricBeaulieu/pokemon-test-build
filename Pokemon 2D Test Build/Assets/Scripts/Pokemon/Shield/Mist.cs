using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : ShieldBase
{
    public Mist(ItemBase item)
    {
        shield = ShieldType.Mist;
        NewShield(null);
    }

    public override bool PreventsStatLoss()
    {
        return true;
    }

    public override string StartMessage(bool isPlayer)
    {
        if (isPlayer == true)
        {
            return "Your team became shrouded in a mist";
        }
        return "Your foe's team became shrouded in a mist";
    }

    public override string EndMessage(bool isPlayer)
    {
        if (isPlayer == true)
        {
            return "Your team's mist wore off!";
        }
        return "Your foe's mist wore off!";
    }
}
