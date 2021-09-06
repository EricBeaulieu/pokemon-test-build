using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [SerializeField] List<string> lines = new List<string>();

    public List<string> Lines
    {
        get { return lines; }
    }

    public Dialog(string message)
    {
        lines.Clear();
        lines.Add(message);
    }

    public Dialog(List<string> messages)
    {
        lines.Clear();
        lines = messages;
    }
}
