using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusConditionArt : MonoBehaviour
{
    static StatusConditionArt _instance = null;

    [SerializeField] Sprite nothing;

    [SerializeField] Sprite poison;
    [SerializeField] Sprite burn;
    [SerializeField] Sprite sleep;
    [SerializeField] Sprite paralyzed;
    [SerializeField] Sprite frozen;

    public static StatusConditionArt instance
    {
        get { return _instance; }
        set { _instance = value; }
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
    }

    public Sprite ReturnStatusConditionArt(ConditionID currentCondition)
    {
        switch (currentCondition)
        {
            case ConditionID.poison:
                return poison;
            case ConditionID.burn:
                return burn;
            case ConditionID.sleep:
                return sleep;
            case ConditionID.paralyzed:
                return paralyzed;
            case ConditionID.frozen:
                return frozen;
            case ConditionID.toxicPoison:
                return poison;
        }
        return nothing;
    }
}
