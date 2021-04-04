using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityID
{
    NA,
    Blaze,Guts,Torrent,Overgrown
}

public class AbilityDB
{

    public static void Initialization()
    {
        //foreach (var kvp in AbilityDex)
        //{
        //    var abilityID = kvp.Key;
        //    var ability = kvp.Value;

        //    ability.Id = abilityID;
        //}
    }

    public static Dictionary<AbilityID, Ability> AbilityDex = new Dictionary<AbilityID, Ability>()
    {
        //A
        //B
        {
            AbilityID.Blaze,//When HP is below 1/3rd its maximum, power of Fire-type moves is increased by 50%.
            new Ability()
            {
                Name = "Blaze",
                Description = "Powers up Fire-type moves when the Pokémon's HP is low.",
                BoostACertainTypeInAPinch = (Pokemon attackingPokemon,ElementType attackType) => {
                    if(attackType != ElementType.Fire)
                    {
                        return 1;
                    }

                    if(attackingPokemon.currentHitPoints/attackingPokemon.maxHitPoints <= 1 / 3)
                    {
                        return 1.5f;
                    }

                    return 1;
                }
            }

        },
        //C
        //D
        //E
        //F
        //G
        {
            AbilityID.Guts,//Boosts a stat by 50% when affected with a status condition
            new Ability()//Status Condition: Any, excluding Freeze
            {
                Name = "Guts",
                BoostsAStatWhenAffectedWithAStatusCondition = (ConditionID condition) =>
                {
                    if(condition != ConditionID.NA)
                    {
                        return true;
                    }
                    return false;
                }
            }

        },
        //H
        //I
        //J
        //K
        //L
        //M
        //N
        //O
        {
            AbilityID.Overgrown,//When HP is below 1/3rd its maximum, power of Grass-type moves is increased by 50%.
            new Ability()
            {
                Name = "Overgrow",
                Description = "Powers up Grass-type moves when the Pokémon's HP is low.",
                BoostACertainTypeInAPinch = (Pokemon attackingPokemon,ElementType attackType) => {
                    if(attackType != ElementType.Grass)
                    {
                        return 1;
                    }

                    if(attackingPokemon.currentHitPoints/attackingPokemon.maxHitPoints <= 1 / 3)
                    {
                        return 1.5f;
                    }

                    return 1;
                }
            }

        },
        //P
        //Q
        //R
        //S
        //T
        {
            AbilityID.Torrent,//When HP is below 1/3rd its maximum, power of Water-type moves is increased by 50%.
            new Ability()
            {
                Name = "Torrent",
                Description = "Powers up Water-type moves when the Pokémon's HP is low.",
                BoostACertainTypeInAPinch = (Pokemon attackingPokemon,ElementType attackType) => {
                    if(attackType != ElementType.Water)
                    {
                        return 1;
                    }

                    if(attackingPokemon.currentHitPoints/attackingPokemon.maxHitPoints <= 1 / 3)
                    {
                        return 1.5f;
                    }

                    return 1;
                }
            }

        },
        //U
        //V
        //W
        //X
        //Y
        //Z



    };

}
