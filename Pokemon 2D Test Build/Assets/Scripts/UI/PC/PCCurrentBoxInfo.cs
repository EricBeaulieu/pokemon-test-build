using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCCurrentBoxInfo : MonoBehaviour
{
    [SerializeField] Image bannerArt;
    [SerializeField] Text bannerBoxName;


    [SerializeField] Image backgroundArt;

    [SerializeField] PCPokemon[] pCPokemon;

    const int row = 6;
    const int column = 5;

    //public void Initialization()
    //{
    //    SetupPCPokemonNavigation();
    //}

    public PCPokemon GetPokemonAtIndex(int index)
    {
        return pCPokemon[index];
    }

    public void SetupData(PCBoxData currentBox)
    {
        for (int i = 0; i < pCPokemon.Length; i++)
        {
            pCPokemon[i].DepositPokemon(currentBox.PokemonInsideBox[i]);
        }
    }

    //void SetupPCPokemonNavigation()
    //{
    //    for (int i = 0; i < pCPokemon.Length; i++)
    //    {
    //        var navigation = pCPokemon[i].GetComponent<Button>().navigation;

    //        //Up
    //        if (((i+1) / row) <= 0)
    //        {
    //            //navigation.selectOnUp == Banner
    //        }
    //        else
    //        {
    //            navigation.selectOnUp = pCPokemon[i - row].GetComponent<Button>();
    //        }

    //        //Down
    //        if(((i + 1) / row) > column)
    //        {
    //            int k = i % ((i/row) * column);

    //            navigation.selectOnDown = pCPokemon[k].GetComponent<Button>();
    //        }
    //        else
    //        {
    //            //navigation.selectOnDown == Banner
    //        }

    //        //Left
    //        if(i%column == 0)
    //        {
    //            navigation.selectOnLeft = pCPokemon[i+(column-1)].GetComponent<Button>();
    //        }
    //        else
    //        {
    //            navigation.selectOnLeft = pCPokemon[i -1].GetComponent<Button>();
    //        }

    //        //Right
    //        if (i % column == (column - 1))
    //        {
    //            navigation.selectOnLeft = pCPokemon[i - (column - 1)].GetComponent<Button>();
    //        }
    //        else
    //        {
    //            navigation.selectOnLeft = pCPokemon[i + 1].GetComponent<Button>();
    //        }

    //        pCPokemon[i].GetComponent<Button>().navigation = navigation;
    //    }
    //}

}
