using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusConditionArt : MonoBehaviour
{
    static StatusConditionArt _instance = null;

    [SerializeField] Sprite nothing;
    [SerializeField] Sprite blankWhite;
    [SerializeField] Color white;

    [SerializeField] Sprite faintedStatus;
    [SerializeField] Sprite shinyStar;

    [Header("Status")]
    [SerializeField] Sprite conditionPoison;
    [SerializeField] Sprite conditionBurn;
    [SerializeField] Sprite conditionSleep;
    [SerializeField] Sprite conditionParalyzed;
    [SerializeField] Sprite conditionFrozen;

    [Header("Status Animations")]
    [SerializeField] Color conditionPoisonColour;
    [SerializeField] Sprite conditionPoisonBubbles;
    [SerializeField] Color conditionBurnColour;
    [SerializeField] Sprite conditionBurnFlames;
    [SerializeField] Sprite conditionSleepZs;
    [SerializeField] Color conditionParalyzedColour;
    [SerializeField] Sprite[] conditionParalyzedStatic;
    [SerializeField] Color conditionFrozenColour;

    [Header("Gender")]
    [SerializeField] Sprite male;
    [SerializeField] Sprite female;

    [Header("Element")]
    [SerializeField] Sprite nA;
    [SerializeField] Sprite bug;
    [SerializeField] Sprite dark;
    [SerializeField] Sprite dragon;
    [SerializeField] Sprite electric;
    [SerializeField] Sprite fairy;
    [SerializeField] Sprite fighting;
    [SerializeField] Sprite fire;
    [SerializeField] Sprite flying;
    [SerializeField] Sprite ghost;
    [SerializeField] Sprite grass;
    [SerializeField] Sprite ground;
    [SerializeField] Sprite ice;
    [SerializeField] Sprite normal;
    [SerializeField] Sprite poison;
    [SerializeField] Sprite psychic;
    [SerializeField] Sprite rock;
    [SerializeField] Sprite steel;
    [SerializeField] Sprite water;

    [Header("Stat Effects")]
    [SerializeField] Sprite attack;
    [SerializeField] Sprite defense;
    [SerializeField] Sprite specialAttack;
    [SerializeField] Sprite specialDefense;
    [SerializeField] Sprite speed;
    [SerializeField] Sprite mix;
    [SerializeField] Sprite accuracy;
    [SerializeField] Sprite evasion;

    [Header("HitPoint Colours")]
    [SerializeField] Color highHP;
    [SerializeField] Color mediumHP;
    [SerializeField] Color lowHP;

    public static StatusConditionArt instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if(nothing == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing nothing");
        }

        //Status
        if (conditionPoison == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing conditionPoison");
        }
        if (conditionBurn == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing conditionBurn");
        }
        if (conditionSleep == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing conditionSleep");
        }
        if (conditionParalyzed == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing conditionParalyzed");
        }
        if (conditionFrozen == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing conditionFrozen");
        }

        //Gender
        if (male == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing male");
        }
        if (female == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing female");
        }

        //Elemenet
        if (nA == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing nA");
        }
        if (bug == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing bug");
        }
        if (dark == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing dark");
        }
        if (dragon == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing dragon");
        }
        if (electric == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing electric");
        }
        if (fairy == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing fairy");
        }
        if (fighting == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing fighting");
        }
        if (fire == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing fire");
        }
        if (flying == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing flying");
        }
        if (ghost == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing ghost");
        }
        if (grass == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing grass");
        }
        if (ground == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing ground");
        }
        if (ice == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing ice");
        }
        if (normal == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing normal");
        }
        if (poison == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing poison");
        }
        if (psychic == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing psychic");
        }
        if (rock == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing rock");
        }
        if (steel == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing steel");
        }
        if (water == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing water");
        }

        //Stat Effects
        if (attack == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing attack");
        }
        if (defense == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing defense");
        }
        if (specialAttack == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing specialAttack");
        }
        if (specialDefense == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing specialDefense");
        }
        if (speed == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing speed");
        }
        if (mix == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing mix");
        }
        if (accuracy == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing accuracy");
        }
        if (evasion == null)
        {
            Debug.LogWarning($"{this.gameObject}'s StatusConditionArt is missing evasion");
        }
    }

    public Sprite Nothing
    {
        get { return nothing; }
    }

    public Sprite BlankWhite
    {
        get { return blankWhite; }
    }

    public Color PlainWhite
    {
        get { return white; }
    }

    public Sprite FaintedStatus
    {
        get { return faintedStatus; }
    }

    public Sprite ReturnStatusConditionArt(ConditionID currentCondition)
    {
        switch (currentCondition)
        {
            case ConditionID.Poison:
                return conditionPoison;
            case ConditionID.Burn:
                return conditionBurn;
            case ConditionID.Sleep:
                return conditionSleep;
            case ConditionID.Paralyzed:
                return conditionParalyzed;
            case ConditionID.Frozen:
                return conditionFrozen;
            case ConditionID.ToxicPoison:
                return conditionPoison;
        }
        return nothing;
    }

    public Color GetStatusConditionAnimationColour(ConditionID conditionID)
    {
        switch (conditionID)
        {
            case ConditionID.Poison:
                return conditionPoisonColour;
            case ConditionID.Burn:
                return conditionBurnColour;
            case ConditionID.Paralyzed:
                return conditionParalyzedColour;
            case ConditionID.Frozen:
                return conditionFrozenColour;
            case ConditionID.ToxicPoison:
                return conditionPoisonColour;
        }
        return white;
    }

    public Sprite GetRandomParalzedConditionAnimationArt()
    {
        return conditionParalyzedStatic[Random.Range(0, conditionParalyzedStatic.Length)];
    }

    public Sprite ReturnGenderArt(Gender gender)
    {
        switch (gender)
        {
            case Gender.Male:
                return male;
            case Gender.Female:
                return female;
        }
        return nothing;
    }

    public Sprite ReturnElementArt(ElementType type)
    {
        switch (type)
        {
            case ElementType.Bug:
                return bug;
            case ElementType.Dark:
                return dark;
            case ElementType.Dragon:
                return dragon;
            case ElementType.Electric:
                return electric;
            case ElementType.Fairy:
                return fairy;
            case ElementType.Fighting:
                return fighting;
            case ElementType.Fire:
                return fire;
            case ElementType.Flying:
                return flying;
            case ElementType.Ghost:
                return ghost;
            case ElementType.Grass:
                return grass;
            case ElementType.Ground:
                return ground;
            case ElementType.Ice:
                return ice;
            case ElementType.Normal:
                return normal;
            case ElementType.Poison:
                return poison;
            case ElementType.Psychic:
                return psychic;
            case ElementType.Rock:
                return rock;
            case ElementType.Steel:
                return steel;
            case ElementType.Water:
                return water;
            default:
                return nA;
        }
    }

    /// <summary>
    /// if it is mixed pass in NA/HP
    /// </summary>
    /// <param name="stat">Current Stat being Changed</param>
    /// <returns></returns>
    public Sprite ReturnStatusChangesArt(StatAttribute stat)
    {
        switch (stat)
        {
            case StatAttribute.Attack:
                return attack;
            case StatAttribute.Defense:
                return defense;
            case StatAttribute.SpecialAttack:
                return specialAttack;
            case StatAttribute.SpecialDefense:
                return specialDefense;
            case StatAttribute.Speed:
                return speed;
            case StatAttribute.Evasion:
                return evasion;
            case StatAttribute.Accuracy:
                return accuracy;
            default:
                return mix;
        }
    }

    public Color ReturnHitPointsColor(float healthNormalized)
    {
        if(healthNormalized >= HPBar.meduimColourThreshold)
        {
            return highHP;
        }
        else if(healthNormalized < HPBar.meduimColourThreshold && healthNormalized >= HPBar.lowColourThreshold)
        {
            return mediumHP;
        }
        else //healthNormalized < HPBar.lowColourThreshold
        {
            return lowHP;
        }
    }

    public Sprite ReturnShinyStatus(bool shiny)
    {
        if(shiny == true)
        {
            return shinyStar;
        }
        else
        {
            return nothing;
        }
    }
}
