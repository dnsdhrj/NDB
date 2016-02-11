using UnityEngine;
using System.Collections;

public class ItemBuyButton : MonoBehaviour {

    private Item buyingitem;
    private Inventory inventory;

    void Start()
    {
        inventory = GameManager.gm.GetComponentInChildren<Inventory>();
    }

    public void SetItem(Item item)
    {
        buyingitem = item;
    }

    public void OnClick()
    {
        inventory.AddItem(buyingitem);
    }
}
