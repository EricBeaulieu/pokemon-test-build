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
    protected int layers { get { return _currentLayers; } set { _currentLayers = Mathf.Clamp(value, 0, _maxLayers()); } }
    int _currentLayers;
    abstract protected int _maxLayers();
    public virtual void OnEntry(Pokemon pokemon) { }
    public virtual StatBoost OnEntryLowerStat(Pokemon pokemon) { return new StatBoost() { stat = StatAttribute.Speed, boost = 0 }; ; }
    public bool CanBeUsed() { return (_currentLayers >= _maxLayers()); }
}
