using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
    [ReadOnly]
    [SerializeField] string id;

    public void GenerateID()
    {
        id = Guid.NewGuid().ToString();
    }

    void OnValidate()
    {
        if ((gameObject.scene.name != null || gameObject.scene.rootCount != 0) && string.IsNullOrEmpty(id) == true)
        {
            GenerateID();
            Debug.Log(id, gameObject);
        }
    }

    public object CaptureState(bool playerSave = false)
    {
        var state = new Dictionary<string, object>();

        foreach (var saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState(playerSave);
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
