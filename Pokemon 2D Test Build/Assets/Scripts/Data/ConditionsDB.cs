using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionID
{
    //Status
    NA, poison, burn, sleep, paralyzed, frozen, toxicPoison,
    //Volatile Status
    confused,bound,cursed,cursedUser
}

public class ConditionsDB
{
    public static void Initialization()
    {
        foreach (var kvp in Conditions)
        {
            var conditionalID = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionalID;
        }
    }

    public static Dictionary<ConditionID, Condition> Conditions = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.poison,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "has been poisoned",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.poison || conditionID == ConditionID.toxicPoison)
                    {
                        return true;
                    }
                    return false;
                },
                HasConditionMessage = "is already poisoned",
                OnEndTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.maxHitPoints/8);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by poison");
                }
            }

        },
        {
            ConditionID.burn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "has been burned",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.burn)
                    {
                        return true;
                    }
                    return false;
                },
                HasConditionMessage = "is already burnt",
                OnEndTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.maxHitPoints/16);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by burn");
                }
            }

        },
        {
            ConditionID.sleep,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = "has fallen asleep",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.sleep)
                    {
                        return true;
                    }
                    return false;
                },
                HasConditionMessage = "is already Asleep",
                OnStart = (Pokemon pokemon) =>
                {
                    //Sleep For 1-3 turns
                    pokemon.statusTime = Random.Range(1,4);
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.statusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.statusChanges.Enqueue($"{pokemon.currentName} has woken up");
                        return true;
                    }

                    pokemon.statusTime--;
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is sleeping");
                    return false;
                }
            }

        },
        {
            ConditionID.paralyzed,
            new Condition()
            {
                Name = "Paralyzed",
                StartMessage = "has been paralyzed",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.paralyzed)
                    {
                        return true;
                    }
                    return false;
                },
                HasConditionMessage = "is already paralyzed",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(Random.Range(1,5) == 1)
                    {
                        pokemon.statusChanges.Enqueue($"{pokemon.currentName} is paralyzed and cannot move");
                        return false;
                    }
                    return true;
                }
            }

        },
        {
            ConditionID.frozen,
            new Condition()
            {
                Name = "Frozen",
                StartMessage = "has been frozen",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.frozen)
                    {
                        return true;
                    }
                    return false;
                },
                HasConditionMessage = "is already Frozen",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(Random.Range(1,6) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.statusChanges.Enqueue($"{pokemon.currentName} has thawed out");
                        return true;
                    }
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is frozen and cannot move");
                    return false;
                }
            }

        },
        {
            ConditionID.toxicPoison,
            new Condition()
            {
                Name = "ToxicPoison",
                StartMessage = "has been badly poisoned",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.poison || conditionID == ConditionID.toxicPoison)
                    {
                        return true;
                    }
                    return false;
                },
                HasConditionMessage = "is already poisoned",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.statusTime = 0;
                },
                OnEndTurn = (Pokemon pokemon) =>
                {
                    pokemon.statusTime++;
                    pokemon.UpdateHP(pokemon.maxHitPoints/(16/pokemon.statusTime));
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by poison");
                }
            }

        },

        //Volatile Status Conditions

        {
            ConditionID.confused,
            new Condition()
            {
                Name = "Confused",
                StartMessage = "has been confused",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.confused)
                    {
                        return true;
                    }
                    return false;
                },
                HasConditionMessage = "is already Confused",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.volatileStatusTime = Random.Range(2,6);
                    Debug.Log($"{pokemon.currentName} will be confused for {pokemon.volatileStatusTime} turns");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.volatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus(ConditionID.confused);
                        pokemon.statusChanges.Enqueue($"{pokemon.currentName} is no longer confused");
                        return true;
                    }

                    pokemon.volatileStatusTime--;

                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is confused");

                    //50% chance to hurt itself
                    if(Random.value > 0.5f)
                    {
                        //Did not hurt itself
                        return true;
                    }

                    //Hurt By Confusion
                    int damageTaken = Mathf.FloorToInt(((((2 * pokemon.currentLevel) / 5) + 2) * 40 * pokemon.attack / pokemon.defense / 50) + 2);
                    pokemon.UpdateHP(damageTaken);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} hurt itself in its confusion");

                    return false;
                }
            }
        },
        {
            ConditionID.cursed,
            new Condition()
            {
                Name = "Cursed",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.cursed)
                    {
                        return true;
                    }
                    return false;
                },
                HasConditionMessage = "is already Cursed",
                OnEndTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(Mathf.CeilToInt((float)pokemon.maxHitPoints/4f));
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is afflicted by the curse");
                }
            }
        },
        {
            ConditionID.cursedUser,
            new Condition()
            {
                Name = "CursedUser",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.maxHitPoints/2);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} cut its own HP to lay a curse on enemy Pokemon");
                    pokemon.CureVolatileStatus(ConditionID.cursedUser);
                },
            }
        },
    };
}