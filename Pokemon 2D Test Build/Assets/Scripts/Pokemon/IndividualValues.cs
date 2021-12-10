using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IndividualValues : SpecifiedValues<IndividualValues>
{
    const int MINIMUM_IV_VALUE = 1;
    const int MAXIMUM_IV_VALUE = 31;

    public override void SetValues(IndividualValues individualValues = null)
    {
        if (individualValues == null)
        {
            GenerateIVs();
            return;
        }

        hitPoints = individualValues.hitPoints == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.hitPoints;
        attack = individualValues.attack == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.attack;
        defense = individualValues.defense == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.defense;
        specialAttack = individualValues.specialAttack == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.specialAttack;
        specialDefense = individualValues.specialDefense == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.specialDefense;
        speed = individualValues.speed == 0 ? Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE) : individualValues.speed;
    }


    void GenerateIVs()
    {
        hitPoints = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
        attack = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
        defense = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
        specialAttack = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
        specialDefense = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
        speed = Random.Range(MINIMUM_IV_VALUE, MAXIMUM_IV_VALUE);
    }
}
