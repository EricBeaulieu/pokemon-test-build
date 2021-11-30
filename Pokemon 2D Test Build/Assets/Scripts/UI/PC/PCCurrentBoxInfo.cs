using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCCurrentBoxInfo : MonoBehaviour
{
    [SerializeField] PCBanner banner;
    public PCBanner GetBanner { get { return banner; } }

    [SerializeField] Image backgroundArt;

    [SerializeField] PCPokemon[] pCPokemon;

    public Vector3 startingPosition { get; private set; }
    public const int OTHER_BOX_OFFSET = 550;

    void Awake()
    {
        startingPosition = transform.localPosition;
    }

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

    public void AlternateSetPosition(bool right)
    {
        Vector2 endPosition = transform.localPosition;
        if(right)
        {
            endPosition.x += OTHER_BOX_OFFSET;
        }
        else
        {
            endPosition.x -= OTHER_BOX_OFFSET;
        }
        transform.localPosition = endPosition;
    }

    public GameObject GetBannerGameObject()
    {
        return banner.gameObject;
    }
}
