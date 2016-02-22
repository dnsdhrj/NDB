using UnityEngine;
using System.Collections;

public class RoundEventBuyButton : MonoBehaviour {

    private RoundEvent buyingEvent;
    private RoundEventDatabase database;

    void Start()
    {
        database = GameManager.gm.GetComponentInChildren<RoundEventDatabase>();
    }

    public void SetEvent(RoundEvent roundEvent)
    {
        buyingEvent = roundEvent;
    }

    public void OnClick()
    {
        database.AddRoundEvent(buyingEvent);
    }
}
