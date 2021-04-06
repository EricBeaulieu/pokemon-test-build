using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct StandardStats
{
    public int MaxHp { get; private set; }
    public int attack { get; private set; }
    public int defense { get; private set; }
    public int specialAttack { get; private set; }
    public int specialDefense { get; private set; }
    public int speed { get; private set; }

    public StandardStats(int hp,int att,int def,int spAtt,int spDef,int spd)
    {
        MaxHp = hp;
        attack = att;
        defense = def;
        specialAttack = spDef;
        specialDefense = spDef;
        speed = spd;
    }
}

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] GameObject levelUpUI;
    [SerializeField] Text[] statPlus;
    [SerializeField] Text[] statNumber;

    bool _waitingOnUserInput = false;

    private void Awake()
    {
        levelUpUI.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            _waitingOnUserInput = false;
        }
    }

    public IEnumerator DisplayLevelUp(StandardStats StatsBeforeLevelUp,StandardStats StatsAfterLevelUp)
    {
        _waitingOnUserInput = true;

        int difHp = StatsAfterLevelUp.MaxHp - StatsBeforeLevelUp.MaxHp;
        int difAttack = StatsAfterLevelUp.attack - StatsBeforeLevelUp.attack;
        int difDefense = StatsAfterLevelUp.defense - StatsBeforeLevelUp.defense;
        int difSpAttack = StatsAfterLevelUp.specialAttack - StatsBeforeLevelUp.specialAttack;
        int difSpDefense = StatsAfterLevelUp.specialDefense - StatsBeforeLevelUp.specialDefense;
        int difSpeed = StatsAfterLevelUp.speed - StatsBeforeLevelUp.speed;

        SetStatPlus(0, difHp);
        SetStatPlus(1, difAttack);
        SetStatPlus(2, difDefense);
        SetStatPlus(3, difSpAttack);
        SetStatPlus(4, difSpDefense);
        SetStatPlus(5, difSpeed);

        SetStatNumber(0, difHp);
        SetStatNumber(1, difAttack);
        SetStatNumber(2, difDefense);
        SetStatNumber(3, difSpAttack);
        SetStatNumber(4, difSpDefense);
        SetStatNumber(5, difSpeed);

        levelUpUI.SetActive(true);

        while(_waitingOnUserInput == true)
        {
            yield return null;
        }

        _waitingOnUserInput = true;

        SetStatPlus(0, 0);
        SetStatPlus(1, 0);
        SetStatPlus(2, 0);
        SetStatPlus(3, 0);
        SetStatPlus(4, 0);
        SetStatPlus(5, 0);

        SetStatNumber(0, StatsAfterLevelUp.MaxHp);
        SetStatNumber(1, StatsAfterLevelUp.attack);
        SetStatNumber(2, StatsAfterLevelUp.defense);
        SetStatNumber(3, StatsAfterLevelUp.specialAttack);
        SetStatNumber(4, StatsAfterLevelUp.specialDefense);
        SetStatNumber(5, StatsAfterLevelUp.speed);

        while (_waitingOnUserInput == true)
        {
            yield return null;
        }

        levelUpUI.SetActive(false);
    }

    void SetStatPlus(int Pos,int dif)
    {
        if (dif > 0)
        {
            statPlus[Pos].text = "+";
        }
        else
        {
            statPlus[Pos].text = "";
        }
    }

    void SetStatNumber(int Pos, int dif)
    {
        statNumber[Pos].text = $"{dif}";
    }
}
