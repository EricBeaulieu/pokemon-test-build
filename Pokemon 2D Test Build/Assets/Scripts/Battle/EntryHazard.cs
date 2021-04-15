using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryHazard
{
    public EntryHazard(int maxNumberOflayers = 1)
    {
        _currentLayers = 0;
        _maxLayers = maxNumberOflayers;
        //OnStart = () => { this.layers++; };
    }

    public EntryHazardID Id { get; set; }
    public string Name { get; set; }
    public Func<BattleUnit,string> StartMessage { get; set; }
    int _currentLayers;
    int _maxLayers;
    public Action<EntryHazard> OnStart { get; set; }
    public Action<Pokemon> OnEntry { get; set; }
    public Func<Pokemon,StatBoost> OnEntryLowerStat { get; set; }

    public int layers
    {
        get { return _currentLayers; }
        set
        {
            if (_currentLayers > _maxLayers)
            {
                _currentLayers = _maxLayers;
            }
            _currentLayers = value;
        }
    }

    public bool CanBeUsed()
    {
        if(layers >=_maxLayers)
        {
            return false;
        }
        return true;
    }
}
