using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Text _dialogBox;
    [SerializeField] int lettersPerSecond = 30;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionsText;
    [SerializeField] List<Text> moveText;

    bool _waitingOnUserInput = false;

    public void SetDialogText(string dialog)
    {
        _dialogBox.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog,bool makeUserWait = false)
    {
        _dialogBox.text = "";

        foreach(char letter in dialog.ToCharArray())
        {
            _dialogBox.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        yield return new WaitForSeconds(1f);
        if (makeUserWait)
        {
            _waitingOnUserInput = true;

            while (_waitingOnUserInput == true)
            {
                yield return null;
            }
        }
    }

    public void BattleStartSetup()
    {
        EnableActionSelector(false);
        EnableMoveSelector(false);
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public bool WaitingOnUserInput
    {
        get { return _waitingOnUserInput; }
        set
        {
            _waitingOnUserInput = value;
        }
    }
}
