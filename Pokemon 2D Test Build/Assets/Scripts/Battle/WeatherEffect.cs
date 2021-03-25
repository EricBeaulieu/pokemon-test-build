using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffect
{
    public WeatherEffectID Id { get; set; }
    public string Name { get; set; }
    public string StartMessage { get; set; }
    public int OnStartDuration { get; set; }
    public Func<BattleSystem,string> OnEndTurn { get; set; }
    public Action<Pokemon> OnEndTurnDamage { get; set; }
}
