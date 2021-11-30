using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PCBanner : MonoBehaviour, ISelectHandler
{
    [SerializeField] Image bannerBackground;
    [SerializeField] Text bannerName;

    public void OnSelect(BaseEventData eventData)
    {
        PCSystem.pointer.MoveToPosition(transform.position);
        PCSystem.pCPokemonDataDisplay.SetupData(null);
    }

    public void SetupBanner(PCBoxData box)
    {
        bannerName.text = $"{box.boxName}";
        //bannerBackground.sprite = sprite;
    }
}
