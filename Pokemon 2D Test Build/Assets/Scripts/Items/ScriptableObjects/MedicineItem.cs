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

    public override void UseItem()
    {
        //
    }
}
