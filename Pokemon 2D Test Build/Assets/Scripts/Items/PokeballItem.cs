using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokeballCaptureID { Regular, Great,Ultra,Master,Net,Dive,Nest,Repeat,Timer}

[CreateAssetMenu(menuName = "Item/Create New PokeBall Entry")]
public class PokeballItem : ItemBase
{
    public PokeballItem()
    {
        itemType = itemType.Pokeball;
    }

    [SerializeField] PokeballCaptureID currentPokeball;

    [SerializeField] Sprite closed;
    [SerializeField] Sprite halfopen;
    [SerializeField] Sprite open;

    public override void UseItem()
    {
        //nothing yet
    }

    public PokeballCaptureID CaptureRate
    {
        get { return currentPokeball; }
    }

    public Sprite[] CaptureSprites()
    {
        return new[] {closed, halfopen, open};
    }

}
