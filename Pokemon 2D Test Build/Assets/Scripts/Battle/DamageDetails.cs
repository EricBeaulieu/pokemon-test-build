using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetails
{
    public bool hasFainted { get; set; }
    public float criticalHit { get; set; }
    public float typeEffectiveness { get; set; }
    public bool abilityActivation { get; set; }
    public bool damageNullified { get; set; }

    public DamageDetails()
    {
        hasFainted = false;
        criticalHit = 1;
        typeEffectiveness = 1;
        abilityActivation = false;
        damageNullified = false;
    }
}
