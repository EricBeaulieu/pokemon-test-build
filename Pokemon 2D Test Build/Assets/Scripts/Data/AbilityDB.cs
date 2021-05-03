using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityID
{
    NA,
    //A
    //B
    BattleArmor, BigPecks, Blaze,
    //C
    Chlorophyll, ClearBody, Competitive,
    //D
    Defiant, Drizzle, Drought,
    //E
    //F
    FlareBoost,FullMetalBody, FurCoat,
    //G
    Guts,
    //H
    HugePower, HyperCutter,
    //I
    IceScales, Intimidate,
    //J
    //K
    KeenEye,
    //L
    //M
    MarvelScale,
    //N
    //O
    Overgrown,
    //P
    PurePower,
    //Q
    QuickFeet,
    //R
    //S
    SandStream, ShellArmor, SnowWarning, Swarm,
    //T
    Torrent,ToxicBoost,
    //U
    //V
    //W
    WhiteSmoke,
    //X
    //Y
    //Z
    
}

public class AbilityDB
{

    public static void Initialization()
    {
        foreach (var kvp in AbilityDex)
        {
            var abilityID = kvp.Key;
            var ability = kvp.Value;

            ability.Id = abilityID;

            if(ability.Name == null)
            {
                Debug.LogWarning($"{ability.Id} name is not preset");
            }

            if (ability.Description == null)
            {
                Debug.LogWarning($"{ability.Id} Description is not preset");
            }
        }
    }

    public static Dictionary<AbilityID, Ability> AbilityDex = new Dictionary<AbilityID, Ability>()
    {
        //A
        //B
        {
            AbilityID.BattleArmor,
            new Ability()
            {
                Name = "Battle Armor",
                Description = "Hard armor protects the Pok�mon from critical hits.",
                PreventsCriticalHits = true
            }
        },
        {
            AbilityID.BigPecks,
            new Ability()
            {
                Name = "Big Pecks",
                Description = "Protects the Pok�mon from Defense-lowering effects.",
                PreventStatFromBeingLowered = (StatAttribute currentStat)=>
                {
                    if(currentStat == StatAttribute.Defense)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName}'s Big Pecks prevents Defense loss";
                }
            }
        },
        {
            AbilityID.Blaze,//When HP is below 1/3rd its maximum, power of Fire-type moves is increased by 50%.
            new Ability()
            {
                Name = "Blaze",
                Description = "Powers up Fire-type moves when the Pok�mon's HP is low.",
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
        {
            AbilityID.Chlorophyll,
            new Ability()
            {
                Name = "Chlorophyll",
                Description = "Boosts the Pok�mon's Speed stat in sunshine.",
                DoublesSpeedInAWeatherEffect = (WeatherEffectID currentWeather) =>
                {
                    if(currentWeather == WeatherEffectID.Sunshine)
                    {
                        return 2;
                    }
                    return 1;
                }
            }
        },
        {
            AbilityID.ClearBody,
            new Ability()
            {
                Name = "Clear Body",
                Description = "Prevents other Pok�mon's moves or Abilities from lowering the Pok�mon's stats.",
                PreventStatFromBeingLowered = (StatAttribute currentStat)=>
                {
                    if(currentStat != StatAttribute.NA)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName}'s Clear Body prevents stat loss";
                }
            }
        },
        {
            AbilityID.Competitive,
            new Ability()
            {
                Name = "Competitive",
                Description = "Boosts the Pok�mon's Special Attack stat sharply when its stats are lowered.",
                BoostStatSharplyIfAnyStatLowered = new StatBoost() { stat = StatAttribute.SpecialAttack, boost = 2 }
            }
        },
        //D
        {
            AbilityID.Defiant,
            new Ability()
            {
                Name = "Defiant",
                Description = "Boosts the Pok�mon's Attack stat sharply when its stats are lowered.",
                BoostStatSharplyIfAnyStatLowered = new StatBoost() { stat = StatAttribute.Attack, boost = 2 }
            }
        },
        {
            AbilityID.Drizzle,//Activates a weather effect for five turns upon entry
            new Ability()
            {
                Name = "Drizzle",
                Description = "The Pok�mon makes it rain when it enters a battle.",
                OnStartWeatherEffect = WeatherEffectID.Rain
            }
        },
        {
            AbilityID.Drought,//Activates a weather effect for five turns upon entry
            new Ability()
            {
                Name = "Drought",
                Description = "Turns the sunlight harsh when the Pok�mon enters a battle.",
                OnStartWeatherEffect = WeatherEffectID.Sunshine
            }
        },
        //E
        //F
        {
            AbilityID.FlareBoost,//Boosts a stat by 50% when affected with a status condition
            new Ability()//Status Condition: Burn
            {
                Name = "Flare Boost",
                Description = "Powers up special attacks when the Pok�mon is burned.",
                BoostsAStatWhenAffectedWithAStatusCondition = (ConditionID condition,StatAttribute benefitialStat) =>
                {
                    if(condition == ConditionID.burn && benefitialStat == StatAttribute.SpecialAttack)
                    {
                        return 1.5f;
                    }
                    return 1;
                },
            }
        },
        {
            AbilityID.FullMetalBody,
            new Ability()
            {
                Name = "Full Metal Body",
                Description = "Prevents other Pok�mon's moves or Abilities from lowering the Pok�mon's stats.",
                PreventStatFromBeingLowered = (StatAttribute currentStat)=>
                {
                    if(currentStat != StatAttribute.NA)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName}'s Full Metal Body prevents stat loss";
                }
            }
        },
        {
            AbilityID.FurCoat,//Doubles the Pok�mon's Attack stat.
            new Ability()
            {
                Name = "Fur Coat",
                Description = "Halves the damage from physical moves.",
                DoublesAStat = (StatAttribute stat) =>
                {
                    if(stat == StatAttribute.Defense)
                    {
                        return 2;
                    }
                    return 1;
                }
            }
        },
        //G
        {
            AbilityID.Guts,//Boosts a stat by 50% when affected with a status condition
            new Ability()//Status Condition: Any, excluding Freeze
            {
                Name = "Guts",
                Description = "It's so gutsy that having a status condition boosts the Pok�mon's Attack stat.",
                BoostsAStatWhenAffectedWithAStatusCondition = (ConditionID condition,StatAttribute benefitialStat) =>
                {
                    if(condition != ConditionID.NA && condition != ConditionID.frozen && benefitialStat == StatAttribute.Attack)
                    {
                        return 1.5f;
                    }
                    return 1;
                },
                NegatesStatusEffectStatDropFromCondition = (ConditionID condition,StatAttribute stat) =>
                {
                    if(condition == ConditionID.burn && stat == StatAttribute.Attack)
                    {
                        return true;
                    }
                    return false;
                }
            }

        },
        //H
        {
            AbilityID.HugePower,//Doubles the Pok�mon's Attack stat.
            new Ability()
            {
                Name = "Huge Power",
                Description = "Doubles the Pok�mon's Attack stat.",
                DoublesAStat = (StatAttribute stat) =>
                {
                    if(stat == StatAttribute.Attack)
                    {
                        return 2;
                    }
                    return 1;
                }
            }
        },
        {
            AbilityID.HyperCutter,
            new Ability()
            {
                Name = "Hyper Cutter",
                Description = "The Pok�mon's proud of its powerful pincers. They prevent other Pok�mon from lowering its Attack stat.",
                PreventStatFromBeingLowered = (StatAttribute currentStat)=>
                {
                    if(currentStat == StatAttribute.Attack)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName}'s Hyper Cutter prevents Attack loss";
                }
            }
        },
        //I
        {
            AbilityID.IceScales,//Doubles the Pok�mon's Special Defense stat.
            new Ability()
            {
                Name = "Ice Scales",
                Description = "The Pok�mon is protected by ice scales, which halve the damage taken from special moves.",
                DoublesAStat = (StatAttribute stat) =>
                {
                    if(stat == StatAttribute.SpecialDefense)
                    {
                        return 2;
                    }
                    return 1;
                }
            }
        },
        {
            AbilityID.Intimidate,
            new Ability()
            {
                Name = "Intimidate",
                Description = "The Pok�mon intimidates opposing Pok�mon upon entering battle, lowering their Attack stat.",
                OnEntryLowerStat = new StatBoost() { stat = StatAttribute.Attack, boost = -1 }
            }
        },
        //J
        //K
        {
            AbilityID.KeenEye,
            new Ability()
            {
                Name = "Keen Eye",
                Description = "Keen eyes prevent other Pok�mon from lowering this Pok�mon's accuracy.",
                PreventStatFromBeingLowered = (StatAttribute currentStat)=>
                {
                    if(currentStat == StatAttribute.Accuracy)
                    {
                        return true;
                    }
                    return false;
                },
                IgnoreStatIncreases = (StatAttribute currentStat) =>
                {
                    if(currentStat == StatAttribute.Evasion)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName}'s Keen Eye prevents Accuracy loss";
                }
            }
        },
        //L
        //M
        {
            AbilityID.MarvelScale,//Boosts a stat by 50% when affected with a status condition
            new Ability()//Status Condition: Any
            {
                Name = "Marvel Scale",
                Description = "The Pok�mon's marvelous scales boost the Defense stat if it has a status condition.",
                BoostsAStatWhenAffectedWithAStatusCondition = (ConditionID condition,StatAttribute benefitialStat) =>
                {
                    if(condition != ConditionID.NA && benefitialStat == StatAttribute.Defense)
                    {
                        return 1.5f;
                    }
                    return 1;
                },
            }
        },
        //N
        //O
        {
            AbilityID.Overgrown,//When HP is below 1/3rd its maximum, power of Grass-type moves is increased by 50%.
            new Ability()
            {
                Name = "Overgrow",
                Description = "Powers up Grass-type moves when the Pok�mon's HP is low.",
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
        {
            AbilityID.PurePower,//Doubles the Pok�mon's Attack stat.
            new Ability()
            {
                Name = "Pure Power",
                Description = "Using its pure power, the Pok�mon doubles its Attack stat.",
                DoublesAStat = (StatAttribute stat) =>
                {
                    if(stat == StatAttribute.Attack)
                    {
                        return 2;
                    }
                    return 1;
                }
            }
        },
        //Q
        {
            AbilityID.QuickFeet,//Boosts a stat by 50% when affected with a status condition
            new Ability()//Status Condition: Any
            {
                Name = "Quick Feet",
                Description = "Boosts the Speed stat if the Pok�mon has a status condition.",
                BoostsAStatWhenAffectedWithAStatusCondition = (ConditionID condition,StatAttribute benefitialStat) =>
                {
                    if(condition != ConditionID.NA && benefitialStat == StatAttribute.Speed)
                    {
                        return 1.5f;
                    }
                    return 1;
                },
                NegatesStatusEffectStatDropFromCondition = (ConditionID condition,StatAttribute stat) =>
                {
                    if(condition == ConditionID.paralyzed && stat == StatAttribute.Speed)
                    {
                        return true;
                    }
                    return false;
                }
            }
        },
        //R
        //S
        {
            AbilityID.SandStream,//Activates a weather effect for five turns upon entry
            new Ability()
            {
                Name = "Sand Stream",
                Description = "The Pok�mon summons a sandstorm when it enters a battle.",
                OnStartWeatherEffect = WeatherEffectID.Sandstorm
            }
        },
        {
            AbilityID.ShellArmor,
            new Ability()
            {
                Name = "Shell Armor",
                Description = "A hard shell protects the Pok�mon from critical hits.",
                PreventsCriticalHits = true
            }
        },
        {
            AbilityID.SnowWarning,//Activates a weather effect for five turns upon entry
            new Ability()
            {
                Name = "Snow Warning",
                Description = "The Pok�mon summons a hailstorm when it enters a battle.",
                OnStartWeatherEffect = WeatherEffectID.Hail
            }
        },
        {
            AbilityID.Swarm,//When HP is below 1/3rd its maximum, power of Bug-type moves is increased by 50%.
            new Ability()
            {
                Name = "Swarm",
                Description = "Powers up Bug-type moves when the Pok�mon's HP is low.",
                BoostACertainTypeInAPinch = (Pokemon attackingPokemon,ElementType attackType) => {
                    if(attackType != ElementType.Bug)
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
        //T
        {
            AbilityID.Torrent,//When HP is below 1/3rd its maximum, power of Water-type moves is increased by 50%.
            new Ability()
            {
                Name = "Torrent",
                Description = "Powers up Water-type moves when the Pok�mon's HP is low.",
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
        {
            AbilityID.ToxicBoost,//Boosts a stat by 50% when affected with a status condition
            new Ability()//Status Condition: Any
            {
                Name = "Toxic Boost",
                Description = "Powers up physical attacks when the Pok�mon is poisoned.",
                BoostsAStatWhenAffectedWithAStatusCondition = (ConditionID condition,StatAttribute benefitialStat) =>
                {
                    if(condition == ConditionID.poison || condition == ConditionID.toxicPoison && benefitialStat == StatAttribute.Attack)
                    {
                        return 1.5f;
                    }
                    return 1;
                },
            }
        },
        //U
        //V
        //W
        {
            AbilityID.WhiteSmoke,
            new Ability()
            {
                Name = "White Smoke",
                Description = "The Pok�mon is protected by its white smoke, which prevents other Pok�mon from lowering its stats.",
                PreventStatFromBeingLowered = (StatAttribute currentStat)=>
                {
                    if(currentStat != StatAttribute.NA)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName}'s White Smoke prevents stat loss";
                }
            }
        },
        //X
        //Y
        //Z
    };
}
