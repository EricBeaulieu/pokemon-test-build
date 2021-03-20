using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    public ConditionID Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }
    public Func<ConditionID, bool> HasCondition { get; set; }
    public string HasConditionMessage { get; set; }
    public Action<Pokemon> OnStart { get; set; }
    public Func<Pokemon,bool> OnBeforeMove { get; set; }
    public Action<Pokemon> OnEndTurn { get; set; }
}
