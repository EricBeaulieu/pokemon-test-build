using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPC : MonoBehaviour, IInteractable
{
    public IEnumerator OnInteract(Vector2 vector2)
    {
        GameManager.instance.GetPCSystem.OpenSystem();
        Debug.Log("Called");
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
