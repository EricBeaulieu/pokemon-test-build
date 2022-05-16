using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecializedMovesHelper : MonoBehaviour
{
    void Start()
    {
        SpecializedMoves.Initialization(this);
        struggle = Resources.Load<MoveBase>("Moveset/Physical/Struggle");

        //Physical
        acrobatics = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        assurance = Resources.Load<MoveBase>("Moveset/Physical/Assurance");
        avalanche = Resources.Load<MoveBase>("Moveset/Physical/Avalanche");
        bounce = Resources.Load<MoveBase>("Moveset/Physical/Bounce");
        brickBreak = Resources.Load<MoveBase>("Moveset/Physical/BrickBreak");
        counter = Resources.Load<MoveBase>("Moveset/Physical/Counter");
        crushGrip = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        cut = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        dig = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        dive = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        earthquake = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        endeavor = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        facade = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        fakeOut = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        fellStinger = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        firstImpression = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        fissure = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        flail = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        fly = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        focusPunch = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        foulPlay = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        freezeShock = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        furyCutter = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        gyroBall = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        highJumpKick = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        jumpKick = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        knockOff = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        lastResort = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        magnitude = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        payback = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        payDay = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        phantomForce = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        powerTrip = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        psychicFangs = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        punishment = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        rage = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        revenge = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        reversal = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        seismicToss = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        shadowForce = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        smackDown = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        smellingSalts = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        solarBlade = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        stompingTantrum = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        suckerPunch = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        superFang = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        thousandArrows = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        throatChop = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        tripleKick = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        wakeUpSlap = Resources.Load<MoveBase>("Moveset/Physical/Acrobatics");
        //Destroy(this);
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
    public MoveBase payDay;
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
    public MoveBase auroraVeil;
    public MoveBase bellyDrum;
    public MoveBase captivate;
    public MoveBase disable;
    public MoveBase encore;
    public MoveBase lightScreen;
    public MoveBase mist;
    public MoveBase moonlight;
    public MoveBase morningSun;
    public MoveBase noRetreat;
    public MoveBase purify;
    public MoveBase rest;
    public MoveBase reflect;
    public MoveBase synthesis;
    public MoveBase shoreUp;
}
