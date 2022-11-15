using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create New Medicine")]
public class MedicineItem : ItemBase
{
    [Header("Medicine Attributes")]
    [SerializeField] int hpRecovered;
    [SerializeField] int ppRecovered;
    public int PpRecovered { get { return ppRecovered; } }
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
        if(pokemon.currentHitPoints <= 0)
        {
            if (revive > 0)
            {
                pokemon.currentHitPoints = Mathf.CeilToInt(pokemon.maxHitPoints * revive);
                return true;
            }
        }
        else
        {
            if (hpRecovered > 0)
            {
                if (pokemon.currentHitPoints < pokemon.maxHitPoints)
                {
                    pokemon.UpdateHPRestored(hpRecovered);
                    return true;
                }
            }

            if (ppRecovered > 0)
            {
                if (multipleMoves == true)
                {
                    for (int i = 0; i < pokemon.moves.Count; i++)
                    {
                        pokemon.moves[i].pP += ppRecovered;
                    }
                    return true;
                }
                else
                {
                    return true;
                }
                //TO DO
                // open up UI showing current moves that can be recovered and show one
            }

            if (specificStatusRecovered != ConditionID.NA)
            {
                if (pokemon.status.Id == specificStatusRecovered)
                {
                    pokemon.CureStatus();
                    return true;
                }
            }

            if (cureAllStatus == true)
            {
                if (pokemon.status != null)
                {
                    pokemon.CureStatus();
                    return true;
                }
            }
        }

        return false;
    }

    public override bool UseItemOption()
    {
        return true;
    }

    public bool IsSingleItemPPRecovery()
    {
        return (multipleMoves == false && ppRecovered > 0);
    }
}
