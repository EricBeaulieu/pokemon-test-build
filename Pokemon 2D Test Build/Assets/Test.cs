using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public ExperienceGroup testExperienceGroup;
    public NatureBase testNature;

	// Use this for initialization
	void Start () {
        TestNatureName(testNature);
	}

    void TestNatureName(NatureBase currentNature)
    {
        Debug.Log(currentNature.natureName);
    }


    void TestExpTable()
    {
        for (int i = 1; i < 100; i++)
        {
            Debug.Log("Level " + i + "EXP to next level " + ExperienceTable.ReturnExperienceRequiredToNextLevel(i, testExperienceGroup));
        }
    }
}
