using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCCurrentBoxInfo : MonoBehaviour
{
    [SerializeField] PCBanner banner;
    [SerializeField] Image backgroundArt;

    [SerializeField] PCPokemon[] pCPokemon;

    const int row = 6;
    const int column = 5;

    //public void Initialization()
    //{
    //    SetupPCPokemonNavigation();
    //}

    public PCPokemon GetPCPokemonAtIndex(int index)
    {
        return pCPokemon[index];
    }

    public void SetupData(PCBoxData currentBox)
    {
        for (int i = 0; i < pCPokemon.Length; i++)
        {
            pCPokemon[i].DepositPokemon(currentBox.PokemonInsideBox[i]);
        }
        banner.SetupBanner(currentBox);
    }
}
