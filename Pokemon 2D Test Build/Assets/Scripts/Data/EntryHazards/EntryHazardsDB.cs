using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntryHazardID
{
    NA,Spikes,ToxicSpikes,StealthRock,StickyWeb
}

public class EntryHazardsDB : MonoBehaviour
{
    public static void Initialization(List<EntryHazardBase> entryHazardBases)
    {
        foreach (EntryHazardBase hazard in entryHazardBases)
        {
            EntryHazards.Add(hazard.Id, hazard);
        }
    }

    static Dictionary<EntryHazardID, EntryHazardBase> EntryHazards = new Dictionary<EntryHazardID, EntryHazardBase>();

    public static EntryHazardBase GetEntryHazardBase(EntryHazardID iD)
    {
        return EntryHazards[iD].ReturnDerivedClassAsNew();
    }
}
