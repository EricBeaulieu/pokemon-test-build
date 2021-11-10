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
    const int row = 6;
    const int column = 5;

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
        Vector2 endPosition = transform.position;
        if(right)
        {
            endPosition.x += OTHER_BOX_OFFSET;
        }
        else
        {
            endPosition.x -= OTHER_BOX_OFFSET;
        }
        transform.position = endPosition;
    }

    public GameObject GetBannerGameObject()
    {
        return banner.gameObject;
    }
}
