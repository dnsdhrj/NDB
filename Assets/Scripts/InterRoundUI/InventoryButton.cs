using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryButton : MonoBehaviour {

    private Inventory inventory;
    private Text buttonText;

	void Start ()
    {
        inventory = GameManager.gm.GetComponentInChildren<Inventory>();
        buttonText = GetComponentInChildren<Text>();
        inventory.inventoryButtonClicked = false;
        buttonText.text = "인벤토리 열기";
        
	}

    public void OnClick()
    {
        if (inventory.inventoryButtonClicked)
        {
            buttonText.text = "인벤토리 열기";
        }
        else
        {
            buttonText.text = "인벤토리 닫기";
        }

        inventory.inventoryButtonClicked = !inventory.inventoryButtonClicked;
        
    }

}
