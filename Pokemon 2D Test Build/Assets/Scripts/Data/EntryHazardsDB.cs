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
            new EntryHazard()
            {
                Name = "Spikes",
                StartMessage = "has been poisoned",
                layers = 0,
                OnStart = () =>
                {
                    
                },
                OnEntry = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.maxHitPoints/8);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt by poison");
                }
            }

        },
    };
}
