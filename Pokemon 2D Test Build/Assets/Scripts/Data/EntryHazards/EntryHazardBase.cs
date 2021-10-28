using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntryHazardBase
{
    public abstract EntryHazardID Id { get; }
    public abstract EntryHazardBase ReturnDerivedClassAsNew();
    public string Name { get { return Id.ToString(); } }
    public abstract string StartMessage(BattleUnit battleUnit);
    protected int layers { get { return currentLayers; } set { currentLayers = Mathf.Clamp(currentLayers, 0, maxLayers()); } }
    int currentLayers;
    abstract protected int maxLayers();
    public virtual void OnEntry(BattleUnit pokemon) { }
    public virtual StatBoost OnEntryLowerStat(BattleUnit defendingUnit) { return null; }
    public bool CanBeUsed()
    {
        if((layers >= maxLayers()))
        {
            return false;
        }
        layers++;
        return true;
    }
}
