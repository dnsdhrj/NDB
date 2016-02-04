using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item{

    public string itemName;
    public int itemID;
    public string itemDescription;
    public Texture2D itemImage;

    public Item(string name, int ID, string desc) {
        itemName = name;
        itemID = ID;
        itemDescription = desc;
        itemImage = Resources.Load<Texture2D>("Item Icons/" + name);
    }

    public Item() {

    }
}
