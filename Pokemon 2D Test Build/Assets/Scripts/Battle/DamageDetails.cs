using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetails
{
    public bool hasFainted { get; set; }
    public float criticalHit { get; set; }
    public float typeEffectiveness { get; set; }
    public bool defendersAbilityActivation { get; set; }
    public bool attackersAbilityActivation { get; set; }
    public bool damageNullified { get; set; }
    public bool oneHitKOMove { get; set; }
    public List<StatBoost> defendersStatBoostByAbility { get; set; }
    public List<StatBoost> attackersStatBoostByDefendersAbility { get; set; }
    public List<StatBoost> alterStatAfterTakingDamage { get; set; }

    public void Clear()
    {
        hasFainted = false;
        criticalHit = 1;
        typeEffectiveness = 1;
        defendersAbilityActivation = false;
        attackersAbilityActivation = false;
        damageNullified = false;
        oneHitKOMove = false;
        defendersStatBoostByAbility = new List<StatBoost>();
        attackersStatBoostByDefendersAbility = new List<StatBoost>();
        alterStatAfterTakingDamage = new List<StatBoost>();
    }
}
