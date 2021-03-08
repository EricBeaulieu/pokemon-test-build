using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySystem : MonoBehaviour
{
    [SerializeField] Text messageBox;

    PartyMemberUI[] _partyMemberSlots;
    [SerializeField] Button cancelButton;

    void Awake()
    {
        _partyMemberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SelectFirstBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_partyMemberSlots[0].gameObject);
        SetMessageText("Choose a Pokemon");
    }

    public void SetPartyData(List<Pokemon> currentParty)
    {
        for (int i = 0; i < _partyMemberSlots.Length; i++)
        {
            if(i < currentParty.Count)
            {
                _partyMemberSlots[i].gameObject.SetActive(true);
                _partyMemberSlots[i].SetData(currentParty[i], i);

                if(i+1 == currentParty.Count)
                {
                    //have to pull the variable out, then change it and set it back in
                    //_partyMemberSlots[i].GetComponent<Button>().navigation.selectOnDown = cancelButton;
                    var navigation = _partyMemberSlots[i].GetComponent<Button>().navigation;
                    navigation.selectOnDown = cancelButton;
                    _partyMemberSlots[i].GetComponent<Button>().navigation = navigation;

                    //CancelButton Up
                    navigation = cancelButton.GetComponent<Button>().navigation;
                    navigation.selectOnUp = _partyMemberSlots[i].GetComponent<Button>();
                    cancelButton.GetComponent<Button>().navigation = navigation;
                }
                else
                {
                    var navigation = _partyMemberSlots[i].GetComponent<Button>().navigation;
                    navigation.selectOnDown = _partyMemberSlots[i+1].GetComponent<Button>();
                    _partyMemberSlots[i].GetComponent<Button>().navigation = navigation;
                }
            }
            else
            {
                _partyMemberSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetMessageText(string dialog)
    {
        messageBox.text = dialog;
    }

}
