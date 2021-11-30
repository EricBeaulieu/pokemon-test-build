using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PurchaseArrows : MonoBehaviour, ISelectHandler
{
    [SerializeField] int incrementAmount;
    ShopSystem shopSystem;

    private void Start()
    {
        shopSystem = GameManager.instance.GetShopSystem;
    }
    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(shopSystem.IncreasePurchaseAmount(incrementAmount));
    }
}
