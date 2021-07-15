using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create New Medicine")]
public class MedicineItem : ItemBase
{
    [Header("Medicine Attributes")]
    [SerializeField] int hpRecovered;
    [SerializeField] int ppRecovered;
    [SerializeField] bool multipleMoves;
    [SerializeField] ConditionID specificStatusRecovered;
    [SerializeField] bool cureAllStatus;
    [Range(0,1)]
    [SerializeField] float revive;

    public MedicineItem()
    {
        itemType = itemType.Medicine;
    }

    public override bool UseItem(Pokemon pokemon)
    {
        if(hpRecovered > 0)
        {
            if(pokemon.currentHitPoints < pokemon.maxHitPoints)
            {
                return true;
            }
        }

        if (ppRecovered > 0)
        {
            if(multipleMoves == true)
            {
                for (int i = 0; i < pokemon.moves.Count; i++)
                {
                    pokemon.moves[i].pP += ppRecovered;
                }
                return true;
            }
            //TO DO
            return false; 
        }

        if(specificStatusRecovered != ConditionID.NA)
        {
            if(pokemon.status.Id == specificStatusRecovered)
            {
                return true;
            }
        }

        if(cureAllStatus == true)
        {
            if(pokemon.status != null)
            {
                return true;
            }
        }

        if(revive > 0 && pokemon.currentHitPoints <= 0)
        {
            pokemon.currentHitPoints = Mathf.CeilToInt(pokemon.maxHitPoints * revive);
            return true;
        }
        return false;
    }

    public override bool UseItemOption()
    {
        return true;
    }
}
