using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public int slotX;
    public int slotY;
    public List<Item> inventory = new List<Item>();
    public List<Item> slots = new List<Item>();
    public GUISkin skin;

    private ItemDatabase database;
    private int inventorySize;
    private bool showInventory = false;
	
	void Start () {
        inventorySize = slotX * slotY;

        for (int i = 0; i < inventorySize; i++) {
            slots.Add(new Item());
        }

        database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
        inventory.Add(database.itemDatabase[0]);
	}

    void Update() {
        if (Input.GetButtonDown("Inventory")){
            showInventory = !showInventory;
        }
    }

    void OnGUI() {
        GUI.skin = skin;
        if (showInventory) {
            DrawInventory();
        }
    }

    void DrawInventory() {
        for (int x = 0; x < slotX; x++) {
            for (int y = 0; y < slotY; y++) {
                GUI.Box(new Rect(x * 60, y * 60, 50, 50), "", skin.GetStyle("Slots"));
            }
        }
    }
}
