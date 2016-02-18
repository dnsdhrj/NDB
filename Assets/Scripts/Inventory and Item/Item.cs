using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item{

    public string itemName;
    public int itemID;
    public int itemPrice;
    public float itemDuration;
    public string itemDescription;
    public delegate IEnumerator VoidCoroutine(Item item);
    public VoidCoroutine itemfunc;
    public Texture2D itemImage;

    //이미지는 아이템이름(itemName)과 똑같은 파일명을 가진 걸 자동으로 사용하도록 했음
    public Item(string name, int ID, int price, float duration, string desc, VoidCoroutine onstart)
    {
        itemName = name;
        itemID = ID;
        itemPrice = price;
        itemDuration = duration;
        itemDescription = desc;
        itemImage = Resources.Load<Texture2D>("Item Icons/" + name);
        itemfunc = onstart;

    }

    public Item()
    {

    }
}
