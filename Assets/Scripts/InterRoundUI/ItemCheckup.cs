using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemCheckup : MonoBehaviour {

    public GameObject itemListTemplate;

    public int numOfShowingItem = 3;

    private GameObject itemPanel;
    private GameObject itemScrollPanel;
    private RectTransform itemscrollPanelrect;

    private Inventory inventory;
    private ItemDatabase database;

    private List<int> randomItemID;

    private int imageIconSize = 256;


    // Use this for initialization
    void Start()
    {
        inventory = GameManager.gm.GetComponentInChildren<Inventory>();
        database = GameManager.gm.GetComponentInChildren<ItemDatabase>();
        itemPanel = GameObject.Find("ItemPanel");
        itemScrollPanel = GameObject.Find("ItemScrollPanel");
        itemscrollPanelrect = itemScrollPanel.GetComponent<RectTransform>();
        itemPanel.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        itemPanel.SetActive(false);
        MakeItemList();
    }

    void MakeItemList()
    {
        SetRandomItemID();
        SetListSize(itemscrollPanelrect);
        List<GameObject> itemList = new List<GameObject>();

        for (int i=0; i<numOfShowingItem; i++)
        {
            int randomID = Random.Range(0, randomItemID.Count);
            Item item = database.itemDatabase[randomItemID[randomID]];
            randomItemID.RemoveAt(randomID);

            GameObject newItem = Instantiate(itemListTemplate, new Vector3(0f, (itemscrollPanelrect.rect.height / 2) - 120f * i - 20f, 0f), Quaternion.identity) as GameObject;
            newItem.name = item.itemName;

            /* 다른 children이 추가되면 아래 코드에서 에러가 발생할 수도? */
            newItem.GetComponentInChildren<Image>().sprite = Sprite.Create(item.itemImage, new Rect(0, 0, imageIconSize, imageIconSize), new Vector2(0f, 0f));
            newItem.transform.SetParent(itemScrollPanel.transform, false);

            newItem.GetComponentInChildren<Text>().text = inventory.CreateTooltip(item);
            newItem.GetComponentInChildren<ItemBuyButton>().SetItem(item);

            itemList.Add(newItem);
        }
    }

    public void ShowItems()
    {
        //이유는 모르겠지만 처음에 위치 조정을 안해주면 스크롤바랑 이미지 표시가 이상해짐
        itemPanel.GetComponent<ScrollRect>().verticalScrollbar.value = 0;
        itemscrollPanelrect.localPosition = new Vector2(itemscrollPanelrect.localPosition.x, -itemscrollPanelrect.rect.height / 2);
        itemPanel.SetActive(true);
    }

    void SetListSize(RectTransform rect) {
        rect.sizeDelta = new Vector2(rect.rect.width, numOfShowingItem * 120f + 20f);
    }

    void SetRandomItemID()
    {
        randomItemID = new List<int>();

        for (int i = 0; i < database.itemDatabase.Count; i++)
        {
            randomItemID.Add(i);
        }
    }
}
