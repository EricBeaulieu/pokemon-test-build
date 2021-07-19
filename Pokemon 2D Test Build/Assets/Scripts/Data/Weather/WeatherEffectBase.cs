using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeatherEffectBase
{
    public abstract WeatherEffectID Id { get; }
    public string Name { get { return Id.ToString(); } }
    public abstract string StartMessage();
    public int duration { get; set; }
    public abstract string OnEndTurn();
    public virtual void OnEndTurnDamage(Pokemon pokemon) { }
}
