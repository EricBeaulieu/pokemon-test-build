using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : CoreSystem
{
    DialogBox currentDialogBox;

    public static int lettersPerSecond { get; private set; } = 30;
    string message;

    bool playerSpedUp;

    const string indicatorWhenWaitingOnInput = " >";

    public override void Initialization()
    {
        SetCurrentDialogBox(dialogBox);
        if(currentDialogBox.gameObject.activeInHierarchy == true)
        {
            currentDialogBox.ShowDialogBox(false);
        }
    }

    public override void HandleUpdate()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            playerSpedUp = true;
        }
    }

    public void SetDialogText(string dialog)
    {
        currentlyDisplaying = dialog;
    }

    public IEnumerator ShowDialogBox(Dialog dialog,Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog();

        currentDialogBox.ShowDialogBox(true);
        for (int i = 0; i < dialog.Lines.Count; i++)
        {
            yield return TypeDialog(dialog.Lines[i],true);
        }

        OnCloseDialog();
        onFinished?.Invoke();
    }

    public void ShowMessage(string message, Action onFinished = null)
    {
        StartCoroutine(ShowDialogBox(new Dialog(message)));
    }

    public IEnumerator TypeDialog(string line, bool makeUserWait = false)
    {
        currentlyDisplaying = "";

        foreach (char letter in line.ToCharArray())
        {
            currentlyDisplaying += letter;

            if(playerSpedUp == true)
            {
                currentlyDisplaying = line;
                yield return TypingDialogEnd(makeUserWait);
                yield break;
            }
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        yield return TypingDialogEnd(makeUserWait);
    }

    IEnumerator TypingDialogEnd(bool waitingForinput)
    {
        yield return new WaitForSeconds(1f);
        if(waitingForinput == true)
        {
            currentlyDisplaying += indicatorWhenWaitingOnInput;
        }

        while (waitingForinput == true)
        {
            if (Input.GetButtonDown("Fire1") | Input.GetButtonDown("Fire2"))
            {
                waitingForinput = false;
            }
            yield return null;
        }
        playerSpedUp = false;
    }

    void OnShowDialog()
    {
        GameManager.SetGameState(GameState.Dialog);
        playerSpedUp = false;
    }

    void OnCloseDialog()
    {
        if (BattleSystem.inBattle == true)
        {
            GameManager.SetGameState(GameState.Battle);
        }
        else
        {
            GameManager.SetGameState(GameState.Overworld);
        }
        currentDialogBox.ShowDialogBox(false);
        SetCurrentDialogBox();
    }

    public void SetCurrentDialogBox(DialogBox newDialogBox = null)
    {
        if(newDialogBox == null)
        {
            currentDialogBox = dialogBox;
        }
        else
        {
            currentDialogBox = newDialogBox;
        }
    }

    string currentlyDisplaying
    {
        get { return message; }
        set
        {
            message = value;
            currentDialogBox.SetDialogText(message);
        }
    }

    public IEnumerator AfterDialogWait()
    {
        currentlyDisplaying += indicatorWhenWaitingOnInput;
        bool waitingForinput = true;

        while (waitingForinput == true)
        {
            if (Input.anyKeyDown)
            {
                waitingForinput = false;
            }
            yield return null;
        }
    }

    public IEnumerator SetChoiceBox(Action onYesSelected, Action onNoSelected = null)
    {
        yield return currentDialogBox.SetChoiceBox(onYesSelected, onNoSelected);
    }
}
