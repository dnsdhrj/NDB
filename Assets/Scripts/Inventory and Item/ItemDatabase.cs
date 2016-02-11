using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
    public List<Item> itemDatabase = new List<Item>();

    //아이템 데이터베이스는 여기에 추가
    void Start()
    {
        itemDatabase.Add(new Item("Dotory", 0, 100, "맛있는 도토리"));
        itemDatabase.Add(new Item("Nokyong", 1, 200, "맛있는 녹용"));
    }


    //아이템 효과는 여기에 추가
    public void useItem(Item item) {
        switch (item.itemID)
        {
            case 0 :
                Debug.Log("도토리 마시쪙");
                break;

            case 1:
                Debug.Log("녹용 맛업쪙");
                break;

            default:
                break;
        }
    }
}
