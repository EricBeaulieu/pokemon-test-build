using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionID
{
    //Status
    NA, Poison, Burn, Sleep, Paralyzed, Frozen, ToxicPoison,
    //Volatile Status
    Confused, Bound, Cursed, CursedUser, Flinch,
    Infatuation,
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
            ConditionID.Poison,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "has been poisoned",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.Poison || conditionID == ConditionID.ToxicPoison)
                    {
                        return true;
                    }
                    return false;
                },
                HasConditionMessage = "is already poisoned",
                OnEndTurn = (Pokemon pokemon) =>
                {
                    int damage = Mathf.FloorToInt(pokemon.maxHitPoints/8);

                    if(damage <=0)
                    {
                        damage = 1;
                    }

                    pokemon.UpdateHP(damage);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by poison");
                }
            }

        },
        {
            ConditionID.Burn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "has been burned",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.Burn)
                    {
                        return true;
                    }
                    return false;
                },
                StatEffectedByCondition = (ConditionID condition,StatAttribute currentStat) =>
                {
                    if(condition == ConditionID.Burn && currentStat == StatAttribute.Attack)
                    {
                        return 0.5f;
                    }
                    return 1;
                },
                HasConditionMessage = "is already burnt",
                OnEndTurn = (Pokemon pokemon) =>
                {
                    int damage = pokemon.maxHitPoints/16;

                    if(damage <=0)
                    {
                        damage = 1;
                    }

                    pokemon.UpdateHP(damage);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by burn");
                }
            }

        },
        {
            ConditionID.Sleep,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = "has fallen asleep",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.Sleep)
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
            ConditionID.Paralyzed,
            new Condition()
            {
                Name = "Paralyzed",
                StartMessage = "has been paralyzed",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.Paralyzed)
                    {
                        return true;
                    }
                    return false;
                },
                StatEffectedByCondition = (ConditionID condition,StatAttribute currentStat) =>
                {
                    if(condition == ConditionID.Paralyzed && currentStat == StatAttribute.Speed)
                    {
                        return 0.5f;
                    }
                    return 1;
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
            ConditionID.Frozen,
            new Condition()
            {
                Name = "Frozen",
                StartMessage = "has been frozen",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.Frozen)
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
            ConditionID.ToxicPoison,
            new Condition()
            {
                Name = "ToxicPoison",
                StartMessage = "has been badly poisoned",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.Poison || conditionID == ConditionID.ToxicPoison)
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
                    int damage = pokemon.maxHitPoints/(16/pokemon.statusTime);

                    if(damage <=0)
                    {
                        damage = 1;
                    }

                    pokemon.UpdateHP(damage);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by poison");
                }
            }

        },

        //Volatile Status Conditions

        {
            ConditionID.Confused,
            new Condition()
            {
                Name = "Confused",
                StartMessage = "has been confused",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.Confused)
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
                        pokemon.CureVolatileStatus(ConditionID.Confused);
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
                    int damage = Mathf.FloorToInt(((((2 * pokemon.currentLevel) / 5) + 2) * 40 * pokemon.attack / pokemon.defense / 50) + 2);

                    if(damage <=0)
                    {
                        damage = 1;
                    }

                    pokemon.UpdateHP(damage);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} hurt itself in its confusion");

                    return false;
                }
            }
        },
        {
            ConditionID.Cursed,
            new Condition()
            {
                Name = "Cursed",
                HasCondition = (ConditionID conditionID) =>
                {
                    if(conditionID == ConditionID.Cursed)
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
            ConditionID.CursedUser,
            new Condition()
            {
                Name = "CursedUser",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.maxHitPoints/2);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} cut its own HP to lay a curse on enemy Pokemon");
                    pokemon.CureVolatileStatus(ConditionID.CursedUser);
                },
            }
        },
        {
            ConditionID.Flinch,
            new Condition()
            {
                Name = "Flinch",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                        pokemon.statusChanges.Enqueue($"{pokemon.currentName} flinched and couldn't move");
                        return false;
                },
                OnEndTurn = (Pokemon pokemon) =>
                {
                    pokemon.CureVolatileStatus(ConditionID.Flinch);
                }
            }
        },
        {
            ConditionID.Infatuation,
            new Condition()
            {
                Name = "Infatuation",
                StartMessage = "fell in love!",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is in love");

                    //50% chance to not attack
                    if(Random.value > 0.5f)
                    {
                        return true;
                    }

                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is immobilized by love ");
                    return false;
                }
            }
        },
    };
}