﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public bool testEXPGroup;
    public ExperienceGroup testExperienceGroup;
    public NatureBase testNature;

    public ElementType attacktype = ElementType.Bug;
    public ElementType defenderType = ElementType.Dark;
    public ElementType defenderType2 = ElementType.NA;

    // Use this for initialization
    void Start () {

        if (testEXPGroup)
        {
            TestExpTable();
        }
        //TestNatureName(testNature);
        //TestTypeChart();
	}

    void TestNatureName(NatureBase currentNature)
    {
        Debug.Log(currentNature.natureName);
    }

    //void TestTypeChart()
    //{
    //    float effectiveness = DamageModifiers.TypeChartEffectiveness(defenderType, attacktype);
    //    Debug.Log($"{defenderType} DefenderType + {attacktype} AttackType = {effectiveness}");
    //}

    void TestExpTable()
    {
        for (int i = 1; i < 101; i++)
        {
            Debug.Log("Level " + i + "EXP to level " + ExperienceTable.ReturnExperienceRequiredForLevel(i, testExperienceGroup));
        }
    }
}
