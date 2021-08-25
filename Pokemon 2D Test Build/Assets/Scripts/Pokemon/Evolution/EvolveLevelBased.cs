using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EvolveLevelBased : EvolutionBase
{
    [SerializeField] int levelRequirement;
    [SerializeField] bool maxFriendShipRequired;

    public override bool CanEvolve(Pokemon pokemon,ItemBase item)
    {
        if(maxFriendShipRequired == true)
        {
            //If max friendship then go into
        }
        return (pokemon.currentLevel >= levelRequirement);
    }
}
