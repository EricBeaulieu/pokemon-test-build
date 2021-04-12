using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    static DialogManager _instance = null;

    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    [SerializeField] int lettersPerSecond = 30;

    public Action OnShowDialog;
    public Action OnCloseDialog;
    Action onDialogFinished;

    Dialog _currentDialog;
    int _currentLine = 0;

    bool _playerSpedUp;
    bool _currentlyTyping;
    bool _waitingOnUserInput;

    public static string indicatorWhenWaitingOnInput = " >";

    public static DialogManager instance
    {
        get { return _instance; }
        private set { _instance = value; }
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

        if(dialogBox.activeInHierarchy == true)
        {
            dialogBox.SetActive(false);
        }

        OnShowDialog += () =>
        {
            _playerSpedUp = false;
            _currentlyTyping = false;
            _waitingOnUserInput = false;
        };
        OnCloseDialog += () => { dialogBox.SetActive(false); };
    }

    public void HandleUpdate()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if (_currentlyTyping == true)
            {
                _playerSpedUp = true;
            }
            else if(_waitingOnUserInput == true && _currentlyTyping == false)
            {
                _currentLine++;
                _waitingOnUserInput = false;

                if (_currentLine < _currentDialog.Lines.Count)
                {
                    StartCoroutine(TypeDialog(_currentDialog.Lines[_currentLine]));
                }
                else
                {
                    OnCloseDialog?.Invoke();
                    onDialogFinished?.Invoke();
                }
            }
        }
    }

    public IEnumerator ShowDialogBox(Dialog dialog,Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        _currentDialog = dialog;
        _currentLine = 0;

        onDialogFinished = onFinished;

        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(_currentDialog.Lines[_currentLine]));
    }

    public void SetDialogText(string line)
    {
        dialogText.text = line;
    }

    public IEnumerator TypeDialog(string line)
    {
        _currentlyTyping = true;
        dialogText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;

            if(_currentlyTyping == true && _playerSpedUp == true)
            {
                SetDialogText(line);
                yield return TypingDialogEnd();
                yield break;
            }
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        yield return TypingDialogEnd();
    }

    IEnumerator TypingDialogEnd()
    {
        _currentlyTyping = false;
        _playerSpedUp = false;
        yield return new WaitForSeconds(0.5f);
        dialogText.text += indicatorWhenWaitingOnInput;
        _waitingOnUserInput = true;
    }
}
