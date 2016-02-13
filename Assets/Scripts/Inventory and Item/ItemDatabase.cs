using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
    public List<Item> itemDatabase = new List<Item>();

    //아이템 데이터베이스는 여기에 추가
    void Start()
    {
        itemDatabase.Add(new Item("도토리", 0, 100, "맛있는 도토리"));
        itemDatabase.Add(new Item("녹용", 1, 200, "맛있는 녹용"));
        itemDatabase.Add(new Item("노란비서", 2, 300, "통학생들의 필수 아이템"));
        itemDatabase.Add(new Item("백세주", 3, 400, "취중코딩"));
        itemDatabase.Add(new Item("용마제구검", 4, 500, "이름 쓰다 자꾸 틀림"));
        itemDatabase.Add(new Item("집행검", 5, 9999, "내 집 마련의 꿈"));
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
