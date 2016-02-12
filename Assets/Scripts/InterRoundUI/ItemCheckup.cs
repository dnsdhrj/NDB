using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemCheckup : MonoBehaviour {

    public GameObject itemPanel;
    public GameObject itemListTemplate;

    private Inventory inventory;
    private ItemDatabase database;

    private int imageIconSize = 256; 
    

	// Use this for initialization
	void Start ()
    {
        inventory = GameManager.gm.GetComponentInChildren<Inventory>();
        database = GameManager.gm.GetComponentInChildren<ItemDatabase>();
        itemPanel = GameObject.Find("ItemPanel");
        itemPanel.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        itemPanel.SetActive(false);
	}

    public void ShowItems() {

        itemPanel.SetActive(true);

        foreach (Item item in database.itemDatabase)
        {
            List<GameObject> itemList  = new List<GameObject>();

            GameObject newItem = Instantiate(itemListTemplate, new Vector3(0f, -120f * item.itemID, 0f), Quaternion.identity) as GameObject;
            newItem.name = item.itemName;

            /* 다른 children이 추가되면 아래 코드에서 에러가 발생할 수도? */
            newItem.GetComponentInChildren<Image>().sprite = Sprite.Create(item.itemImage, new Rect(0, 0, imageIconSize, imageIconSize), new Vector2(0f, 0f));
            newItem.transform.SetParent(GameObject.Find("ItemPanel").transform, false);
            newItem.GetComponentInChildren<Text>().text = inventory.CreateTooltip(item);
            newItem.GetComponentInChildren<ItemBuyButton>().SetItem(item);

            itemList.Add(newItem);
            
        }

    }
}
