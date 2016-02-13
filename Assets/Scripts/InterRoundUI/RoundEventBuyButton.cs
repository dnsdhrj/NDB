using UnityEngine;
using System.Collections;

public class RoundEventBuyButton : MonoBehaviour {

    private RoundEvent buyingEvent;
    //private Inventory inventory;

    void Start()
    {
        //inventory = GameManager.gm.GetComponentInChildren<Inventory>();
    }

    public void SetEvent(RoundEvent roundEvent)
    {
        buyingEvent = roundEvent;
    }

    public void OnClick()
    {
        //inventory.AddItem(buyingitem);
    }
}
