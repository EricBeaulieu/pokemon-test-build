using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExperienceGroup { Erratic, Fast, MediumFast, MediumSlow, Slow, VerySlow}

public static class ExperienceTable {

    static float _expMultiplier;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cL">Current Level</param>
    /// <param name="group">Current Experience Group this belongs in</param>
    /// <returns></returns>
    public static int ReturnExperienceRequiredToNextLevel(int cL, ExperienceGroup group)
    {
        cL++;
        int expForNextLevel;

        switch (group)
        {
            case ExperienceGroup.Erratic:
                if (cL < 100)
                {
                    expForNextLevel = ErraticFormula(cL);
                }
                else
                    return 600000;
                break;
            case ExperienceGroup.Fast:
                if (cL < 100)
                {
                    expForNextLevel = FastFormula(cL);
                }
                else
                    return 800000;
                break;
            case ExperienceGroup.MediumFast:
                if (cL < 100)
                {
                    expForNextLevel = FastFormula(cL);
                }
                else
                    return 1000000;
                expForNextLevel = MediumFastFormula(cL);
                break;
            case ExperienceGroup.MediumSlow:
                if (cL < 100)
                {
                    expForNextLevel = FastFormula(cL);
                }
                else
                    return 1059860;
                expForNextLevel = MediumSlowFormula(cL);
                break;
            case ExperienceGroup.Slow:
                if (cL < 100)
                {
                    expForNextLevel = FastFormula(cL);
                }
                else
                    return 1250000;
                expForNextLevel = SlowFormula(cL);
                break;
            case ExperienceGroup.VerySlow:
                if (cL < 100)
                {
                    expForNextLevel = FastFormula(cL);
                }
                else
                    return 1640000;
                expForNextLevel = VerySlow(cL);
                break;
            default:
                expForNextLevel = 10000000;//To Prevent endless Loop
                Debug.LogError("Out of bounds Error");
                break;
        }

        return expForNextLevel;
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
            return Mathf.CeilToInt(Mathf.Pow(cL, 3) * ((cL + 73) / 150));
        }
        else if(cL > 15 && cL<= 36)
        {
            return Mathf.CeilToInt(Mathf.Pow(cL, 3) * ((cL + 14) / 50));
        }
        else//37 - 100
        {
            return Mathf.CeilToInt(_expMultiplier * Mathf.Pow(cL, 3));
        }
    }
}
