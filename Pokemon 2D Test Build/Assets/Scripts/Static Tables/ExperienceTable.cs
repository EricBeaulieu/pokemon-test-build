using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExperienceGroup { Erratic, Fast, MediumFast, MediumSlow, Slow, VerySlow}

public static class ExperienceTable {

    static float _expMultiplier;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentLevel">Current Level</param>
    /// <param name="group">Current Experience Group this belongs in</param>
    /// <returns></returns>
    public static int ReturnExperienceRequiredForLevel(int currentLevel, ExperienceGroup group)
    {
        switch (group)
        {
            case ExperienceGroup.Erratic:
                if (currentLevel < 100)
                {
                    return Mathf.Clamp(ErraticFormula(currentLevel), 0, 600000);
                }
                else
                    return 600000;
            case ExperienceGroup.Fast:
                if (currentLevel < 100)
                {
                    return Mathf.Clamp(FastFormula(currentLevel),0, 800000);
                }
                else
                    return 800000;
            case ExperienceGroup.MediumFast:
                if (currentLevel < 100)
                {
                    return Mathf.Clamp(MediumFastFormula(currentLevel),0,1000000);
                }
                else
                    return 1000000;
            case ExperienceGroup.MediumSlow:
                if (currentLevel < 100)
                {
                    return Mathf.Clamp(MediumSlowFormula(currentLevel),0, 1059860);
                }
                else
                    return 1059860;
            case ExperienceGroup.Slow:
                if (currentLevel < 100)
                {
                    return Mathf.Clamp(SlowFormula(currentLevel),0, 1250000);
                }
                else
                    return 1250000;
            case ExperienceGroup.VerySlow:
                if (currentLevel < 100)
                {
                    return Mathf.Clamp(VerySlow(currentLevel),0, 1640000);
                }
                else
                    return 1640000;
            default:
                Debug.LogError("Out of bounds Error");
                break;
        }
        return -1;
    }

    static int ErraticFormula(int cL)
    {
        if(cL <= 50)
        {
            return Mathf.CeilToInt(Mathf.Pow(cL, 3) * (100 - cL) / 50);
        }
        else if(cL >50 && cL <= 68)
        {
            return Mathf.CeilToInt(Mathf.Pow(cL, 3) * (150 - cL) / 100);
        }
        else if(cL > 68 && cL <=98)
        {
            return Mathf.CeilToInt(Mathf.Pow(cL, 3) * (1911 - (10*cL)) / 1500);
        }
        else//99-100
        {
            return Mathf.CeilToInt(Mathf.Pow(cL, 3) * (160 - cL) / 100);
        }
    }

    static int FastFormula(int cL)
    {
        _expMultiplier = 0.8f;

        return  Mathf.CeilToInt(_expMultiplier*(Mathf.Pow(cL,3)));
    }

    static int MediumFastFormula(int cL)
    {
        return Mathf.CeilToInt(Mathf.Pow(cL, 3));
    }

    static int MediumSlowFormula(int cl)
    {
        _expMultiplier = 1.2f;

        return Mathf.CeilToInt((_expMultiplier * Mathf.Pow(cl,3)) - (15 * Mathf.Pow(cl,2)) + (100 * cl) - 140);
    }

    static int SlowFormula(int cL)
    {
        _expMultiplier = 1.25f;

        return Mathf.CeilToInt(_expMultiplier * Mathf.Pow(cL, 3));
    }

    static int VerySlow(int cL)
    {
        if(cL <= 15)
        {
            return Mathf.CeilToInt(Mathf.Pow(cL, 3) * ((cL + 73f) / 150f));
        }
        else if(cL > 15 && cL<= 36)
        {
            int number = Mathf.CeilToInt(Mathf.Pow(cL, 3) * ((cL + 14f) / 50f));
            return number;
        }
        else//37 - 100
        {
            return Mathf.CeilToInt(Mathf.Pow(cL, 3) * (((0.5f * cL) + 32)/50));
        }
    }
}
