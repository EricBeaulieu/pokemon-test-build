using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SaveableEntity : MonoBehaviour
{
    [SerializeField] string id;
    [ContextMenu("Generate ID")]
    void GenerateID()
    {
        id = Guid.NewGuid().ToString();
    }

    void Awake()
    {
        if(id == "")
        GenerateID();
    }

    public object CaptureState()
    {
        var state = new Dictionary<string, object>();

        foreach (var saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }

        return state;
    }

    public void RestoreState(object state)
    {
        var stateDictionary = (Dictionary<string, object>)state;

        foreach (var saveable in GetComponents<ISaveable>())
        {
            string typeName = saveable.GetType().ToString();
            if(stateDictionary.TryGetValue(typeName,out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }

    public string GetID
    {
        get { return id; }
    }
}
