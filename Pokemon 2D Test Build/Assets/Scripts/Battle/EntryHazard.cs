using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryHazard
{
    public EntryHazardID Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }
    public int layers { get; set; }
    public Action OnStart { get; set; }
    public Action<Pokemon> OnEntry { get; set; }
}
