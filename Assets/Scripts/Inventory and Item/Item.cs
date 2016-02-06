using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item{

    public string itemName;
    public int itemID;
    public int itemPrice;
    public string itemDescription;
    public Texture2D itemImage;

    //이미지는 아이템이름(itemName)과 똑같은 파일명을 가진 걸 자동으로 사용하도록 했음
    public Item(string name, int ID, int price, string desc)
    {
        itemName = name;
        itemID = ID;
        itemPrice = price;
        itemDescription = desc;
        itemImage = Resources.Load<Texture2D>("Item Icons/" + name);
    }

    public Item()
    {

    }
}
