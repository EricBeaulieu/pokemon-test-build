using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCSystem : CoreSystem
{
    SelectableBoxUI selectableBox;
    [SerializeField] PCCurrentBoxInfo currentBox;
    [SerializeField] int numberOfBoxes = 30;
    PCBoxData[] boxData;

    int _boxIndex = 0;
    int boxIndex
    {
        get { return _boxIndex; }
        set
        {
            _boxIndex += value;
            if (_boxIndex < 0)
            {
                _boxIndex = numberOfBoxes;
            }
            else if (_boxIndex > numberOfBoxes)
            {
                _boxIndex = 0;
            }
        }
    }

    public override void OpenSystem(bool specifiedBool = false)
    {
        gameObject.SetActive(true);
        selectableBox.SelectBox();
        currentBox.SetupData(boxData[boxIndex]);
        //player busy
    }

    protected override void CloseSystem()
    {
        gameObject.SetActive(false);
        //player active
    }

    public override void HandleUpdate()
    {
        if(Input.GetKeyDown("Fire2"))
        {
            CloseSystem();
        }
    }

    public override void Initialization()
    {
        gameObject.SetActive(false);
        boxData = new PCBoxData[numberOfBoxes];
        for (int i = 0; i < boxData.Length; i++)
        {
            boxData[i] = new PCBoxData();
            boxData[i].boxName = $"Box {i + 1}";
        }
        //load all current pokemon inside of the boxes
        //currentBox.Initialization();
        selectableBox = new SelectableBoxUI(currentBox.GetPokemonAtIndex(0).gameObject);
    }
    

}
