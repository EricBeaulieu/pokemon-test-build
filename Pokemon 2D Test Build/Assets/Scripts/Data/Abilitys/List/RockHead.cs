using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHead : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.RockHead; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new RockHead(); }
    public override string Description()
    {
        return "Protects the Pokémon from recoil damage.";
    }
    public override bool PreventsRecoilDamage(MoveBase move)
    {
        if(move.MoveName == "Struggle")
        {
            return false;
        }

        if(move.RecoilPercentage < 100)
        {
            return true;
        }
        return base.PreventsRecoilDamage(move);
    }
}
