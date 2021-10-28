using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecializedMovesHelper : MonoBehaviour
{
    void Start()
    {
        SpecializedMoves.Initialization(this);
        Destroy(this);
    }

    [Header("SpecializedMoves")]
    public MoveBase struggle;

    [Header("Physical")]
    public MoveBase acrobatics;
    public MoveBase assurance;
    public MoveBase avalanche;
    public MoveBase bounce;
    public MoveBase brickBreak;
    public MoveBase counter;
    public MoveBase crushGrip;
    public MoveBase cut;
    public MoveBase dig;
    public MoveBase dive;
    public MoveBase earthquake;
    public MoveBase endeavor;
    public MoveBase facade;
    public MoveBase fakeOut;
    public MoveBase fellStinger;
    public MoveBase firstImpression;
    public MoveBase fissure;
    public MoveBase flail;
    public MoveBase fly;
    public MoveBase focusPunch;
    public MoveBase foulPlay;
    public MoveBase freezeShock;
    public MoveBase furyCutter;
    public MoveBase gyroBall;
    public MoveBase highJumpKick;
    public MoveBase jumpKick;
    public MoveBase knockOff;
    public MoveBase lastResort;
    public MoveBase magnitude;
    public MoveBase payback;
    public MoveBase phantomForce;
    public MoveBase powerTrip;
    public MoveBase psychicFangs;
    public MoveBase punishment;
    public MoveBase rage;
    public MoveBase revenge;
    public MoveBase reversal;
    public MoveBase seismicToss;
    public MoveBase shadowForce;
    public MoveBase smackDown;
    public MoveBase smellingSalts;
    public MoveBase solarBlade;
    public MoveBase stompingTantrum;
    public MoveBase suckerPunch;
    public MoveBase superFang;
    public MoveBase thousandArrows;
    public MoveBase throatChop;
    public MoveBase tripleKick;
    public MoveBase wakeUpSlap;

    [Header("Special")]
    public MoveBase blizzard;
    public MoveBase brine;
    public MoveBase clearSmog;
    public MoveBase dreamEater;
    public MoveBase echoedVoice;
    public MoveBase electroBall;
    public MoveBase eruption;
    public MoveBase gust;
    public MoveBase hex;
    public MoveBase hurricane;
    public MoveBase psywave;
    public MoveBase round;
    public MoveBase shellTrap;
    public MoveBase snore;
    public MoveBase solarBeam;
    public MoveBase storedPower;
    public MoveBase surf;
    public MoveBase synchronoise;
    public MoveBase thunder;
    public MoveBase trumpCard;
    public MoveBase twister;
    public MoveBase venoshock;
    public MoveBase waterSpout;
    public MoveBase weatherBall;
    public MoveBase whirlpool;
    public MoveBase wringOut;

    [Header("Status")]
    public MoveBase acupressure;
    public MoveBase noRetreat;
    public MoveBase encore;
    public MoveBase synthesis;
    public MoveBase moonlight;
    public MoveBase morningSun;
    public MoveBase shoreUp;
    public MoveBase purify;
    public MoveBase rest;
    public MoveBase disable;
    public MoveBase reflect;
    public MoveBase lightScreen;
    public MoveBase auroraVeil;
    public MoveBase mist;
}
