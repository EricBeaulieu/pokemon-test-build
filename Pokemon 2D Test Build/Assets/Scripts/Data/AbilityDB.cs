using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityID
{
    NA,
    //A
    Adaptability, Aerilate, AirLock,
    //B
    BattleArmor, BigPecks, Blaze,
    //C
    Chlorophyll, ClearBody, CloudNine, Competitive, CuteCharm,
    //D
    Defiant, DragonsMaw, Drizzle, Drought,
    //E
    EffectSpore,
    //F
    FlameBody, FlareBoost, FullMetalBody, FurCoat,
    //G
    Galvanize, Guts,
    //H
    HugePower, HyperCutter,
    //I
    IceScales, Immunity, InnerFocus, Insomnia, Intimidate,
    IronFist,
    //J
    //K
    KeenEye,
    //L
    Limber,
    //M
    MagmaArmor, MarvelScale, MegaLauncher,
    //N
    Neuroforce, Normalize,
    //O
    Oblivious, Overgrown, OwnTempo,
    //P
    Pixilate, PoisonPoint, PunkRock, PurePower,
    //Q
    QuickFeet,
    //R
    Reckless, Refrigerate,
    //S
    SandRush, SandStream, ShellArmor, SkillLink, SlushRush,
    SnowWarning, Static, Steelworker, StrongJaw, Swarm,
    SwiftSwim,
    //T
    Technician, TintedLens,Torrent, ToughClaws, Transistor,
    ToxicBoost,
    //U
    //V
    VitalSpirit,
    //W
    WaterVeil, WhiteSmoke,
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
        {
            AbilityID.Adaptability,
            new Ability()
            {
                Name = "Adaptability",
                Description = "Powers up moves of the same type as the Pokémon.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(attackingPokemon.pokemonBase.IsType(currentMove.Type) == true)
                    {
                        return 1.33f;
                    }
                    return 1f;
                }
            }
        },
        {
            AbilityID.Aerilate,
            new Ability()
            {
                Name = "Aerilate",
                Description = "Normal-type moves become Flying-type moves. The power of those moves is boosted a little.",
                ChangeMovesToDifferentTypeAndIncreasesTheirPower = (MoveBase currentMove) =>
                {
                    if(currentMove.Type == ElementType.Normal)
                    {
                        return currentMove.adjustedMove(ElementType.Flying,0.2f);
                    }
                    return currentMove;
                }
            }
        },
        {
            AbilityID.AirLock,
            new Ability()
            {
                Name = "Air Lock",
                Description = "Eliminates the effects of weather.",
                NegatesWeatherEffects = true
            }
        },
        //B
        {
            AbilityID.BattleArmor,
            new Ability()
            {
                Name = "Battle Armor",
                Description = "Hard armor protects the Pokémon from critical hits.",
                PreventsCriticalHits = true
            }
        },
        {
            AbilityID.BigPecks,
            new Ability()
            {
                Name = "Big Pecks",
                Description = "Protects the Pokémon from Defense-lowering effects.",
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
        {
            AbilityID.Chlorophyll,
            new Ability()
            {
                Name = "Chlorophyll",
                Description = "Boosts the Pokémon's Speed stat in sunshine.",
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
                Description = "Prevents other Pokémon's moves or Abilities from lowering the Pokémon's stats.",
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
            AbilityID.CloudNine,
            new Ability()
            {
                Name = "Cloud Nine",
                Description = "Eliminates the effects of weather.",
                NegatesWeatherEffects = true
            }
        },
        {
            AbilityID.Competitive,
            new Ability()
            {
                Name = "Competitive",
                Description = "Boosts the Pokémon's Special Attack stat sharply when its stats are lowered.",
                BoostStatSharplyIfAnyStatLowered = new StatBoost() { stat = StatAttribute.SpecialAttack, boost = 2 }
            }
        },
        {
            AbilityID.CuteCharm,
            new Ability()//30% chance
            {
                Name = "Cute Charm",
                Description = "Contact with the Pokémon may cause infatuation.",
                ContactMoveMayCauseStatusEffect = (Pokemon defendingPokemon,Pokemon attackingPokemon,MoveBase currentAttack) =>
                {
                    if(currentAttack.PhysicalContact == true)
                    {
                        if (attackingPokemon.CheckStatusImmunities(ConditionID.Infatuation) == false)
                        {
                            if(BattleSystem.CheckIfInflatuated(defendingPokemon,attackingPokemon) == true)
                            {
                                if(Random.value < 0.3f)
                                {
                                    return ConditionID.Infatuation;
                                }
                                    
                            }
                        }
                    }
                    return ConditionID.NA;
                }
            }
        },
        //D
        {
            AbilityID.Defiant,
            new Ability()
            {
                Name = "Defiant",
                Description = "Boosts the Pokémon's Attack stat sharply when its stats are lowered.",
                BoostStatSharplyIfAnyStatLowered = new StatBoost() { stat = StatAttribute.Attack, boost = 2 }
            }
        },
        {
            AbilityID.DragonsMaw,
            new Ability()
            {
                Name = "Dragon's Maw",
                Description = "Powers up Dragon-type moves.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(currentMove.Type == ElementType.Dragon)
                    {
                        return 1.5f;
                    }
                    return 1f;
                }
            }
        },
        {
            AbilityID.Drizzle,//Activates a weather effect for five turns upon entry
            new Ability()
            {
                Name = "Drizzle",
                Description = "The Pokémon makes it rain when it enters a battle.",
                OnStartWeatherEffect = WeatherEffectID.Rain
            }
        },
        {
            AbilityID.Drought,//Activates a weather effect for five turns upon entry
            new Ability()
            {
                Name = "Drought",
                Description = "Turns the sunlight harsh when the Pokémon enters a battle.",
                OnStartWeatherEffect = WeatherEffectID.Sunshine
            }
        },
        //E
        {
            AbilityID.EffectSpore,
            new Ability()//30% chance
            {
                Name = "Effect Spore",
                Description = "Contact with the Pokémon may inflict poison, sleep, or paralysis on its attacker.",
                ContactMoveMayCauseStatusEffect = (Pokemon defendingPokemon,Pokemon attackingPokemon,MoveBase currentAttack) =>
                {
                    if(currentAttack.PhysicalContact == true)
                    {
                        float rnd = Random.Range(0,100)/100;
                        ConditionID rndCondition = ConditionID.NA;

                        if(rnd <= 0.09f)
                        {
                            rndCondition = ConditionID.Poison;
                        }
                        else if (rnd > 0.09f && rnd < 0.2f)
                        {
                            rndCondition = ConditionID.Paralyzed;
                        }
                        else if (rnd >= 0.2f && rnd <= 0.3f)
                        {
                            rndCondition = ConditionID.Sleep;
                        }
                        else
                        {
                            return rndCondition;
                        }


                        if (attackingPokemon.CheckStatusImmunities(rndCondition) == false)
                        {
                            return rndCondition;
                        }
                    }

                    return ConditionID.NA;
                }
            }
        },
        //F
        {
            AbilityID.FlameBody,
            new Ability()//30% chance
            {
                Name = "Flame Body",
                Description = "Contact with the Pokémon may burn the attacker.",
                ContactMoveMayCauseStatusEffect = (Pokemon defendingPokemon,Pokemon attackingPokemon,MoveBase currentAttack) =>
                {
                    if(currentAttack.PhysicalContact == true)
                    {
                        if (attackingPokemon.CheckStatusImmunities(ConditionID.Burn) == false)
                        {
                            if(Random.value < 0.3f)
                            {
                                return ConditionID.Burn;
                            }
                        }
                    }
                    return ConditionID.NA;
                }
            }
        },
        {
            AbilityID.FlareBoost,//Boosts a stat by 50% when affected with a status condition
            new Ability()//Status Condition: Burn
            {
                Name = "Flare Boost",
                Description = "Powers up special attacks when the Pokémon is burned.",
                BoostsAStatWhenAffectedWithAStatusCondition = (ConditionID condition,StatAttribute benefitialStat) =>
                {
                    if(condition == ConditionID.Burn && benefitialStat == StatAttribute.SpecialAttack)
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
                Description = "Prevents other Pokémon's moves or Abilities from lowering the Pokémon's stats.",
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
            AbilityID.FurCoat,//Doubles the Pokémon's Attack stat.
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
            AbilityID.Galvanize,
            new Ability()
            {
                Name = "Galvanize",
                Description = "Normal-type moves become Electric-type moves. The power of those moves is boosted a little.",
                ChangeMovesToDifferentTypeAndIncreasesTheirPower = (MoveBase currentMove) =>
                {
                    if(currentMove.Type == ElementType.Normal)
                    {
                        return currentMove.adjustedMove(ElementType.Electric,0.2f);
                    }
                    return currentMove;
                }
            }
        },
        {
            AbilityID.Guts,//Boosts a stat by 50% when affected with a status condition
            new Ability()//Status Condition: Any, excluding Freeze
            {
                Name = "Guts",
                Description = "It's so gutsy that having a status condition boosts the Pokémon's Attack stat.",
                BoostsAStatWhenAffectedWithAStatusCondition = (ConditionID condition,StatAttribute benefitialStat) =>
                {
                    if(condition != ConditionID.NA && condition != ConditionID.Frozen && benefitialStat == StatAttribute.Attack)
                    {
                        return 1.5f;
                    }
                    return 1;
                },
                NegatesStatusEffectStatDropFromCondition = (ConditionID condition,StatAttribute stat) =>
                {
                    if(condition == ConditionID.Burn && stat == StatAttribute.Attack)
                    {
                        return true;
                    }
                    return false;
                }
            }

        },
        //H
        {
            AbilityID.HugePower,//Doubles the Pokémon's Attack stat.
            new Ability()
            {
                Name = "Huge Power",
                Description = "Doubles the Pokémon's Attack stat.",
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
                Description = "The Pokémon's proud of its powerful pincers. They prevent other Pokémon from lowering its Attack stat.",
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
            AbilityID.IceScales,//Doubles the Pokémon's Special Defense stat.
            new Ability()
            {
                Name = "Ice Scales",
                Description = "The Pokémon is protected by ice scales, which halve the damage taken from special moves.",
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
            AbilityID.Immunity,
            new Ability()
            {
                Name = "Immunity",
                Description = "The immune system of the Pokémon prevents it from getting poisoned.",
                PreventCertainStatusCondition = (ConditionID condition) =>
                {
                    if(condition == ConditionID.Poison || condition == ConditionID.ToxicPoison)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName}'s Immunity prevents poisoning";
                }
            }
        },
        {
            AbilityID.InnerFocus,
            new Ability()
            {
                Name = "Inner Focus",
                Description = "The Pokémon's intensely focused, and that protects the Pokémon from flinching.",
                PreventCertainStatusCondition = (ConditionID condition) =>
                {
                    if(condition == ConditionID.Flinch)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName} won't flinch because of its Inner Focus!";
                }
            }
        },
        {
            AbilityID.Insomnia,
            new Ability()
            {
                Name = "Insomnia",
                Description = "The Pokémon is suffering from insomnia and cannot fall asleep.",
                PreventCertainStatusCondition = (ConditionID condition) =>
                {
                    if(condition == ConditionID.Sleep)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName} can't sleep due to Insomnia";
                }
            }
        },
        {
            AbilityID.Intimidate,
            new Ability()
            {
                Name = "Intimidate",
                Description = "The Pokémon intimidates opposing Pokémon upon entering battle, lowering their Attack stat.",
                OnEntryLowerStat = new StatBoost() { stat = StatAttribute.Attack, boost = -1 }
            }
        },
        {
            AbilityID.IronFist,
            new Ability()
            {
                Name = "Iron Fist",
                Description = "Powers up punching moves.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(currentMove.PunchMove == true)
                    {
                        return 1.2f;
                    }
                    return 1f;
                }
            }
        },
        //J
        //K
        {
            AbilityID.KeenEye,
            new Ability()
            {
                Name = "Keen Eye",
                Description = "Keen eyes prevent other Pokémon from lowering this Pokémon's accuracy.",
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
        {
            AbilityID.Limber,
            new Ability()
            {
                Name = "Limber",
                Description = "Its limber body protects the Pokémon from paralysis.",
                PreventCertainStatusCondition = (ConditionID condition) =>
                {
                    if(condition == ConditionID.Paralyzed)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName}'s Limber protects it from paralysis";
                }
            }
        },
        //M
        {
            AbilityID.MagmaArmor,
            new Ability()
            {
                Name = "Magma Armor",
                Description = "The Pokémon is covered with hot magma, which prevents the Pokémon from becoming frozen.",
                PreventCertainStatusCondition = (ConditionID condition) =>
                {
                    if(condition == ConditionID.Frozen)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName} Magma Armor prevents freezing";
                }
            }
        },
        {
            AbilityID.MarvelScale,//Boosts a stat by 50% when affected with a status condition
            new Ability()//Status Condition: Any
            {
                Name = "Marvel Scale",
                Description = "The Pokémon's marvelous scales boost the Defense stat if it has a status condition.",
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
        {
            AbilityID.MegaLauncher,
            new Ability()
            {
                Name = "Mega Launcher",
                Description = "Powers up aura and pulse moves.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(currentMove.MoveName.Contains("Pulse")||currentMove.MoveName.Contains("Aura"))
                    {
                        return 1.5f;
                    }
                    return 1f;
                }
            }
        },
        //N
        {
            AbilityID.Neuroforce,
            new Ability()
            {
                Name = "Neuroforce",
                Description = "Powers up moves that are super effective.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(DamageModifiers.TypeChartEffectiveness(defendingPokemon.pokemonBase, currentMove.Type) > 1)
                    {
                        return 1.25f;
                    }
                    return 1f;
                }
            }
        },
        {
            AbilityID.Normalize,
            new Ability()
            {
                Name = "Normalize",
                Description = "All the Pokémon's moves become Normal type. The power of those moves is boosted a little.",
                ChangeMovesToDifferentTypeAndIncreasesTheirPower = (MoveBase currentMove) =>
                {
                    return currentMove.adjustedMove(ElementType.Normal,0.2f);
                }
            }
        },
        //O
        {
            AbilityID.Oblivious,
            new Ability()
            {
                Name = "Oblivious",
                Description = "The Pokémon is oblivious, and that keeps it from being infatuated or falling for taunts.",
                PreventCertainStatusCondition = (ConditionID condition) =>
                {
                    if(condition == ConditionID.Infatuation)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName} is Oblivious to Inflantuation and Taunts";
                }
            }
        },
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
        {
            AbilityID.OwnTempo,
            new Ability()
            {
                Name = "Own Tempo",
                Description = "This Pokémon has its own tempo, and that prevents it from becoming confused.",
                PreventCertainStatusCondition = (ConditionID condition) =>
                {
                    if(condition == ConditionID.Confused)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName}'s Own Tempo prevents Confusion";
                }
            }
        },
        //P
        {
            AbilityID.Pixilate,
            new Ability()
            {
                Name = "Pixilate",
                Description = "Normal-type moves become Fairy-type moves. The power of those moves is boosted a little.",
                ChangeMovesToDifferentTypeAndIncreasesTheirPower = (MoveBase currentMove) =>
                {
                    if(currentMove.Type == ElementType.Normal)
                    {
                        return currentMove.adjustedMove(ElementType.Fairy,0.2f);
                    }
                    return currentMove;
                }
            }
        },
        {
            AbilityID.PoisonPoint,
            new Ability()//30% chance
            {
                Name = "Poison Point",
                Description = "Contact with the Pokémon may poison the attacker.",
                ContactMoveMayCauseStatusEffect = (Pokemon defendingPokemon,Pokemon attackingPokemon,MoveBase currentAttack) =>
                {
                    if(currentAttack.PhysicalContact == true)
                    {
                        if (attackingPokemon.CheckStatusImmunities(ConditionID.Poison) == false)
                        {
                            if(Random.value < 0.3f)
                            {
                                return ConditionID.Poison;
                            }
                        }
                    }
                    return ConditionID.NA;
                }
            }
        },
        {
            AbilityID.PunkRock,
            new Ability()
            {
                Name = "Punk Rock",
                Description = "Boosts the power of sound-based moves. The Pokémon also takes half the damage from these kinds of moves.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(currentMove.SoundType == true)
                    {
                        return 1.3f;
                    }
                    return 1f;
                }
            }
        },
        {
            AbilityID.PurePower,//Doubles the Pokémon's Attack stat.
            new Ability()
            {
                Name = "Pure Power",
                Description = "Using its pure power, the Pokémon doubles its Attack stat.",
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
                Description = "Boosts the Speed stat if the Pokémon has a status condition.",
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
                    if(condition == ConditionID.Paralyzed && stat == StatAttribute.Speed)
                    {
                        return true;
                    }
                    return false;
                }
            }
        },
        //R
        {
            AbilityID.Reckless,
            new Ability()
            {
                Name = "Reckless",
                Description = "Powers up moves that have recoil damage.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(currentMove.RecoilType != Recoil.NA)
                    {
                        return 1.2f;
                    }
                    return 1f;
                }
            }
        },
        {
            AbilityID.Refrigerate,
            new Ability()
            {
                Name = "Refrigerate",
                Description = "Normal-type moves become Ice-type moves. The power of those moves is boosted a little.",
                ChangeMovesToDifferentTypeAndIncreasesTheirPower = (MoveBase currentMove) =>
                {
                    if(currentMove.Type == ElementType.Normal)
                    {
                        return currentMove.adjustedMove(ElementType.Ice,0.2f);
                    }
                    return currentMove;
                }
            }
        },
        //S
        {
            AbilityID.SandRush,
            new Ability()
            {
                Name = "Sand Rush",
                Description = "Boosts the Pokémon's Speed stat in a sandstorm.",
                DoublesSpeedInAWeatherEffect = (WeatherEffectID currentWeather) =>
                {
                    if(currentWeather == WeatherEffectID.Sandstorm)
                    {
                        return 2;
                    }
                    return 1;
                }
            }
        },
        {
            AbilityID.SandStream,//Activates a weather effect for five turns upon entry
            new Ability()
            {
                Name = "Sand Stream",
                Description = "The Pokémon summons a sandstorm when it enters a battle.",
                OnStartWeatherEffect = WeatherEffectID.Sandstorm
            }
        },
        {
            AbilityID.ShellArmor,
            new Ability()
            {
                Name = "Shell Armor",
                Description = "A hard shell protects the Pokémon from critical hits.",
                PreventsCriticalHits = true
            }
        },
        {
            AbilityID.SkillLink,
            new Ability()
            {
                Name = "Skill Link",
                Description = "Maximizes the number of times multistrike moves hit.",
                MaximizeMultistrikeMovesHit = true
            }
        },
        {
            AbilityID.SlushRush,
            new Ability()
            {
                Name = "Slush Rush",
                Description = "Boosts the Pokémon's Speed stat in a hailstorm.",
                DoublesSpeedInAWeatherEffect = (WeatherEffectID currentWeather) =>
                {
                    if(currentWeather == WeatherEffectID.Hail)
                    {
                        return 2;
                    }
                    return 1;
                }
            }
        },
        {
            AbilityID.SnowWarning,//Activates a weather effect for five turns upon entry
            new Ability()
            {
                Name = "Snow Warning",
                Description = "The Pokémon summons a hailstorm when it enters a battle.",
                OnStartWeatherEffect = WeatherEffectID.Hail
            }
        },
        {
            AbilityID.Static,
            new Ability()//30% chance
            {
                Name = "Static",
                Description = "The Pokémon is charged with static electricity, so contact with it may cause paralysis.",
                ContactMoveMayCauseStatusEffect = (Pokemon defendingPokemon,Pokemon attackingPokemon,MoveBase currentAttack) =>
                {
                    if(currentAttack.PhysicalContact == true)
                    {
                        if (attackingPokemon.CheckStatusImmunities(ConditionID.Paralyzed) == false)
                        {
                            if(Random.value < 0.3f)
                            {
                                return ConditionID.Paralyzed;
                            }
                        }
                    }
                    return ConditionID.NA;
                }
            }
        },
        {
            AbilityID.Steelworker,
            new Ability()
            {
                Name = "Steelworker",
                Description = "Powers up Steel-type moves.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(currentMove.Type == ElementType.Steel)
                    {
                        return 1.5f;
                    }
                    return 1f;
                }
            }
        },
        {
            AbilityID.StrongJaw,
            new Ability()
            {
                Name = "Strong Jaw",
                Description = "The Pokémon's strong jaw boosts the power of its biting moves.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(currentMove.BitingMove == true)
                    {
                        return 1.5f;
                    }
                    return 1f;
                }
            }
        },
        {
            AbilityID.Swarm,//When HP is below 1/3rd its maximum, power of Bug-type moves is increased by 50%.
            new Ability()
            {
                Name = "Swarm",
                Description = "Powers up Bug-type moves when the Pokémon's HP is low.",
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
        {
            AbilityID.SwiftSwim,
            new Ability()
            {
                Name = "Swift Swim",
                Description = "Boosts the Pokémon's Speed stat in rain.",
                DoublesSpeedInAWeatherEffect = (WeatherEffectID currentWeather) =>
                {
                    if(currentWeather == WeatherEffectID.Rain)
                    {
                        return 2;
                    }
                    return 1;
                }
            }
        },
        //T
        {
            AbilityID.Technician,
            new Ability()
            {
                Name = "Technician",
                Description = "Powers up the Pokémon's weaker moves.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(currentMove.MovePower <= 60)
                    {
                        return 1.5f;
                    }
                    return 1f;
                }
            }
        },
        {
            AbilityID.TintedLens,
            new Ability()
            {
                Name = "TintedLens",
                Description = "The Pokémon can use \"not very effective\" moves to deal regular damage.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(DamageModifiers.TypeChartEffectiveness(defendingPokemon.pokemonBase, currentMove.Type) < 1)
                    {
                        return 2f;
                    }
                    return 1f;
                }
            }
        },
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
        {
            AbilityID.ToughClaws,
            new Ability()
            {
                Name = "Tough Claws",
                Description = "Powers up moves that make direct contact.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(currentMove.PhysicalContact == true)
                    {
                        return 1.3f;
                    }
                    return 1f;
                }
            }
        },
        {
            AbilityID.Transistor,
            new Ability()
            {
                Name = "Transistor",
                Description = "Powers up Electric-type moves.",
                PowerUpCertainMoves = (Pokemon attackingPokemon,Pokemon defendingPokemon,MoveBase currentMove) =>
                {
                    if(currentMove.Type == ElementType.Electric)
                    {
                        return 1.5f;
                    }
                    return 1f;
                }
            }
        },
        {
            AbilityID.ToxicBoost,//Boosts a stat by 50% when affected with a status condition
            new Ability()//Status Condition: Any
            {
                Name = "Toxic Boost",
                Description = "Powers up physical attacks when the Pokémon is poisoned.",
                BoostsAStatWhenAffectedWithAStatusCondition = (ConditionID condition,StatAttribute benefitialStat) =>
                {
                    if(condition == ConditionID.Poison || condition == ConditionID.ToxicPoison && benefitialStat == StatAttribute.Attack)
                    {
                        return 1.5f;
                    }
                    return 1;
                },
            }
        },
        //U
        //V
        {
            AbilityID.VitalSpirit,
            new Ability()
            {
                Name = "Vital Spirit",
                Description = "The Pokémon is full of vitality, and that prevents it from falling asleep.",
                PreventCertainStatusCondition = (ConditionID condition) =>
                {
                    if(condition == ConditionID.Sleep)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName} can't sleep due to Vital Spirit";
                }
            }
        },
        //W
        {
            AbilityID.WaterVeil,
            new Ability()
            {
                Name = "Water Veil",
                Description = "The Pokémon is covered with a water veil, which prevents the Pokémon from getting a burn.",
                PreventCertainStatusCondition = (ConditionID condition) =>
                {
                    if(condition == ConditionID.Burn)
                    {
                        return true;
                    }
                    return false;
                },
                OnAbilitityActivation = (Pokemon pokemon) =>
                {
                    return $"{pokemon.currentName}'s Water Veil prevents burning";
                }
            }
        },
        {
            AbilityID.WhiteSmoke,
            new Ability()
            {
                Name = "White Smoke",
                Description = "The Pokémon is protected by its white smoke, which prevents other Pokémon from lowering its stats.",
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
