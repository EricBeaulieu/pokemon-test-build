using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PCTopButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] Button button;
    public event Action ClosePartyScreen;

    public void OnSelect(BaseEventData eventData)
    {
        if(PCParty.isOn == false)
        {
            PCSystem.pointer.MoveToPosition(transform.position, false);
            PCSystem.pCPokemonDataDisplay.SetupData(null);
        }
        else
        {
            ClosePartyScreen?.Invoke();
        }
    }

    public Button GetButton()
    {
        return button;
    }
}
