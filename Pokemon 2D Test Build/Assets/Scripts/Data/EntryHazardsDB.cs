using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntryHazardID
{
    NA,spikes,toxicSpikes,stealthRock,stickyWeb
}

public class EntryHazardsDB : MonoBehaviour
{
    public static void Initialization()
    {
        foreach (var kvp in EntryHazards)
        {
            var entryHazardID = kvp.Key;
            var entryHazard = kvp.Value;

            entryHazard.Id = entryHazardID;
        }
    }

    public static Dictionary<EntryHazardID, EntryHazard> EntryHazards = new Dictionary<EntryHazardID, EntryHazard>()
    {
        {
            EntryHazardID.spikes,
            new EntryHazard(3)
            {
                Name = "Spikes",
                StartMessage = (BattleUnit battleUnit) =>
                {
                    string message = "Spikes were scattered all around the feet of the ";

                    if(battleUnit.isPlayerPokemon)
                    {
                        message += "Player's";
                    }
                    else
                    {
                        message += "foes's";
                    }

                    message += " team!";

                    return message;
                },
                OnStart = (EntryHazard entryHazard) =>
                {
                    if(entryHazard.Id == EntryHazardID.spikes)
                    {
                        entryHazard.layers++;
                    }
                },
                OnEntry = (Pokemon pokemon) =>
                {
                    if (pokemon.pokemonBase.IsType(ElementType.Flying))
                    {
                        return;
                    }

                    int currentLayers = EntryHazards[EntryHazardID.spikes].layers;

                    if(currentLayers == 0)
                    {
                        return;
                    }

                    int damage = pokemon.maxHitPoints/(10 - (currentLayers*2));

                    if(damage <=0)
                    {
                        damage = 1;
                    }

                    pokemon.UpdateHP(damage);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt Spikes");
                }
            }
        },
        {
            EntryHazardID.toxicSpikes,
            new EntryHazard(2)
            {
                Name = "Toxic Spikes",
                StartMessage = (BattleUnit battleUnit) =>
                {
                    string message = "Poisonous Spikes were scattered all around the feet of the ";

                    if(battleUnit.isPlayerPokemon)
                    {
                        message += "Player's";
                    }
                    else
                    {
                        message += "foes's";
                    }

                    message += " team!";

                    return message;
                },
                OnStart = (EntryHazard entryHazard) =>
                {
                    if(entryHazard.Id == EntryHazardID.toxicSpikes)
                    {
                        entryHazard.layers++;
                    }
                },
                OnEntry = (Pokemon pokemon) =>
                {
                    if (pokemon.pokemonBase.IsType(ElementType.Flying) || pokemon.status != null)
                    {
                        return;
                    }

                    if(pokemon.pokemonBase.IsType(ElementType.Poison) || pokemon.pokemonBase.IsType(ElementType.Steel))
                    {
                        pokemon.statusChanges.Enqueue($"{pokemon.currentName} is uneffected by the toxic spikes");
                        return;
                    }

                    int currentLayers = EntryHazards[EntryHazardID.toxicSpikes].layers;

                    if(currentLayers == 0)
                    {
                        return;
                    }
                    else if(currentLayers == 1)
                    {
                        pokemon.SetStatus(ConditionID.poison);
                    }
                    else
                    {
                        pokemon.SetStatus(ConditionID.toxicPoison);
                    }
                }
            }
        },
        {
            EntryHazardID.stealthRock,
            new EntryHazard()
            {
                Name = "Stealth Rock",
                StartMessage = (BattleUnit battleUnit) =>
                {
                    string message = "Pointed Stones are floating in the air of your ";

                    if(battleUnit.isPlayerPokemon == false)
                    {
                        message += "foes's ";
                    }

                    message += "team!";

                    return message;
                },
                OnStart = (EntryHazard entryHazard) =>
                {
                    if(entryHazard.Id == EntryHazardID.stealthRock)
                    {
                        entryHazard.layers++;
                    }
                },
                OnEntry = (Pokemon pokemon) =>
                {
                    float damageEffectiveness = DamageModifiers.TypeChartEffectiveness(pokemon.pokemonBase,ElementType.Rock);

                    int damage = Mathf.FloorToInt(pokemon.maxHitPoints/(8/damageEffectiveness));

                    if(damage <=0)
                    {
                        damage = 1;
                    }

                    pokemon.UpdateHP(damage);
                    pokemon.statusChanges.Enqueue($"Pointed Stones Dug into {pokemon.currentName}");
                }
            }
        },
        {
            EntryHazardID.stickyWeb,
            new EntryHazard()
            {
                Name = "Sticky Web",
                StartMessage = (BattleUnit battleUnit) =>
                {
                    string message = "A sticky web has been laid out all around the ";

                    if(battleUnit.isPlayerPokemon)
                    {
                        message += "Player's";
                    }
                    else
                    {
                        message += "foes's";
                    }

                    message += " team!";

                    return message;
                },
                OnStart = (EntryHazard entryHazard) =>
                {
                    if(entryHazard.Id == EntryHazardID.stickyWeb)
                    {
                        entryHazard.layers++;
                    }
                },
                OnEntry = (Pokemon pokemon) => 
                {
                    if (pokemon.pokemonBase.IsType(ElementType.Flying))
                    {
                        return;
                    }

                    List<StatBoost> speedDown = new List<StatBoost>()
                    {
                        new StatBoost
                        {
                            stat = StatAttribute.Speed,
                            boost = -1
                        }
                    };
                    pokemon.ApplyStatModifier(speedDown);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} was caught in the sticky web");
                }
            }
        }
    };
}
