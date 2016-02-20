using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
    public List<Item> itemDatabase = new List<Item>();
    public bool[] isItemUsing;

    //아이템 데이터베이스는 여기에 추가
    void Start()
    {
        //itemDatabase.Add(new Item("아이템이름", ID, 가격, 지속시간, "설명", 함수명));
        itemDatabase.Add(new Item("도토리", 0, 100, 5f, "도토리를 먹은 다람쥐들이 일시적으로 무적이 됩니다. \n (아이템 사용 시 필드 상에 있는 다람쥐들에만 적용)", Dotory));
        itemDatabase.Add(new Item("녹용", 1, 200, 5f, "녹용을 먹은 유저들이 일시적으로 강해집니다", Nokyong));
        itemDatabase.Add(new Item("초보자용 갑주", 2, 500, 5f, "초보 유저가 중수 유저로 변화하는 정도가 일시적으로 증가합니다.", NoviceArmor));
        itemDatabase.Add(new Item("아싸고도리", 3, 1000, 0f, "사행성 아이템!\n 인기도가 랜덤하게 증가 또는 감소합니다.", AssaGodory));
        //itemDatabase.Add(new Item("노란비서", 2, 300, "통학생들의 필수 아이템"));
        //itemDatabase.Add(new Item("백세주", 3, 400, "취중코딩"));
        //itemDatabase.Add(new Item("용마제구검", 4, 500, "이름 쓰다 자꾸 틀림"));
        itemDatabase.Add(new Item("집행검", 4, 9999, 0f, "득템! 인기도가 1000 증가합니다.", JipHangSword));

        isItemUsing = new bool[itemDatabase.Count];
        isItemUsing.Initialize();
    }


    //아이템 효과는 여기에 추가
    public void useItem(Item item) {
        if (isItemUsing[item.itemID] == true)
        {
            GameManager.gm.ShowMessageBox("이미 해당 아이템이 사용중입니다.");
            return;
        }
        isItemUsing[item.itemID] = true;
        StartCoroutine(item.itemfunc(item));
    }

    IEnumerator Dotory(Item item)
    {
        int numOfDaram = Daram.All.Count;
        float startTime;

        for (int i = 0; i < numOfDaram; i++)
        {
            Daram.All[i].HP = int.MaxValue;
        }

        startTime = GameManager.gm.time;

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (GameManager.gm.time - startTime > item.itemDuration) break;
        }

        for (int i = 0; i < numOfDaram; i++)
        {
            Daram.All[i].HP = Daram.All[i].InitialHP;
        }

        isItemUsing[item.itemID] = false;
    }

    IEnumerator Nokyong(Item item)
    {
        float startTime;

        for(int i = 0; i<User.Count; i++)
            GameManager.gm.userDamagePerLevel[i] *= 2;

        startTime = GameManager.gm.time;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (GameManager.gm.time - startTime > item.itemDuration) break;
        }
        for (int i = 0; i < User.Count; i++)
            GameManager.gm.userDamagePerLevel[i] /= 2;

        isItemUsing[item.itemID] = false;
    }

    IEnumerator JipHangSword(Item item)
    {
        GameManager.gm.fame += 1000;
        isItemUsing[item.itemID] = false;
        yield return new WaitForSeconds(0f);
    }

    IEnumerator NoviceArmor(Item item)
    {
        float startTime = GameManager.gm.time;

        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (GameManager.gm.time - startTime > item.itemDuration) break;
            GameManager.gm.UserLevel2();
        }

        isItemUsing[item.itemID] = false;
    }

    IEnumerator AssaGodory(Item item)
    {
        GameManager.gm.fame += Random.Range(-1000, 1000);
        yield return new WaitForSeconds(0f);
    }
}
