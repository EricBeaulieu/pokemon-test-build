using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBackgroundArt : MonoBehaviour
{
    static PartyBackgroundArt _instance = null;

    [SerializeField] Sprite firstPartyMemberHealthySelectedBackground;
    [SerializeField] Sprite firstPartyMemberHealthyNonSelectedBackground;

    [SerializeField] Sprite firstPartyMemberFaintedSelectedBackground;
    [SerializeField] Sprite firstPartyMemberFaintedNonSelectedBackground;

    [SerializeField] Sprite firstPartyMemberSwitchSelectedBackground;
    [SerializeField] Sprite firstPartyMemberSwitchSourceBackground;

    [SerializeField] Sprite partyMemberHealthySelectedBackground;
    [SerializeField] Sprite partyMemberHealthyNonSelectedBackground;

    [SerializeField] Sprite partyMemberFaintedSelectedBackground;
    [SerializeField] Sprite partyMemberFaintedNonSelectedBackground;

    [SerializeField] Sprite partyMemberSwitchSelectedBackground;
    [SerializeField] Sprite partyMemberSwitchSourceBackground;

    [SerializeField] Sprite itemSprite;

    public static PartyBackgroundArt instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    public Sprite ReturnBackgroundArt(int currentHealth, bool isFirstSlot = false,bool isSelected = false, bool switching = false)
    {
        if(isFirstSlot == true)
        {
            if(switching == true)
            {
                if(isSelected == true)
                {
                    return firstPartyMemberSwitchSelectedBackground;
                }
                else//isSelected == false
                {
                    return firstPartyMemberSwitchSourceBackground;
                }
            }

            if(currentHealth > 0)
            {
                if (isSelected == true)
                {
                    return firstPartyMemberHealthySelectedBackground;
                }
                else//isSelected == false
                {
                    return firstPartyMemberHealthyNonSelectedBackground;
                }
            }
            else
            {
                if (isSelected == true)
                {
                    return firstPartyMemberFaintedSelectedBackground;
                }
                else//isSelected == false
                {
                    return firstPartyMemberFaintedNonSelectedBackground;
                }
            }
        }
        else//Not First Slot
        {
            if (switching == true)
            {
                if (isSelected == true)
                {
                    return partyMemberSwitchSelectedBackground;
                }
                else//isSelected == false
                {
                    return partyMemberSwitchSourceBackground;
                }
            }

            if (currentHealth > 0)
            {
                if (isSelected == true)
                {
                    return partyMemberHealthySelectedBackground;
                }
                else//isSelected == false
                {
                    return partyMemberHealthyNonSelectedBackground;
                }
            }
            else
            {
                if (isSelected == true)
                {
                    return partyMemberFaintedSelectedBackground;
                }
                else//isSelected == false
                {
                    return partyMemberFaintedNonSelectedBackground;
                }
            }
        }
    }

    public Sprite HoldItemSprite()
    {
        return itemSprite;
    }
}
