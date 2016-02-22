using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RoundEventCheckup : MonoBehaviour {

    public GameObject roundEventListTemplate;
    public int numOfShowingRoundEvent = 3;

    private GameObject eventPanel;
    private GameObject eventScrollPanel;
    private RoundEventDatabase database;
    private RectTransform eventScrollPanelrect;
    private int imageIconSize = 256;

    private List<int> RandomRoundEventID;

    void Start ()
    {
        database = GameManager.gm.GetComponentInChildren<RoundEventDatabase>();
        eventPanel = GameObject.Find("EventPanel");
        eventScrollPanel = GameObject.Find("EventScrollPanel");
        eventScrollPanelrect = eventScrollPanel.GetComponent<RectTransform>();
        eventPanel.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        eventPanel.SetActive(false);
        MakeRoundEventList();
    }

    void MakeRoundEventList()
    {
        SetRandomRoundEventID();
        SetListSize(eventScrollPanelrect);
        List<GameObject> eventList = new List<GameObject>();
        for(int i = 0; i< numOfShowingRoundEvent; i++)
        {
            int randomID = Random.Range(0, RandomRoundEventID.Count);
            RoundEvent roundEvent = database.roundEventDatabase[RandomRoundEventID[randomID]];
            RandomRoundEventID.RemoveAt(randomID);

            GameObject newEvent = Instantiate(roundEventListTemplate, new Vector3(0f, (eventScrollPanelrect.rect.height / 2) -120f * i - 20f, 0f), Quaternion.identity) as GameObject;
            newEvent.name = roundEvent.eventName;

            /* 다른 children이 추가되면 아래 코드에서 에러가 발생할 수도? */
            newEvent.GetComponentInChildren<Image>().sprite = Sprite.Create(roundEvent.eventImage, new Rect(0, 0, imageIconSize, imageIconSize), new Vector2(0f, 0f));
            newEvent.transform.SetParent(eventScrollPanel.transform, false);

            newEvent.GetComponentInChildren<Text>().text = CreateEventTooltip(roundEvent);
            newEvent.GetComponentInChildren<RoundEventBuyButton>().SetEvent(roundEvent);

            eventList.Add(newEvent);

        }
    }

    public void ShowRoundEvents()
    {
        //이유는 모르겠지만 처음에 위치 조정을 안해주면 스크롤바랑 이미지 표시가 이상해짐
        eventPanel.GetComponent<ScrollRect>().verticalScrollbar.value = 0;
        eventScrollPanelrect.localPosition = new Vector2(eventScrollPanelrect.localPosition.x, -eventScrollPanelrect.rect.height / 2);
        eventPanel.SetActive(true);

    }

    void SetListSize(RectTransform rect)
    {
        rect.sizeDelta = new Vector2(rect.rect.width, numOfShowingRoundEvent * 120f + 20f);
    }

    public string CreateEventTooltip(RoundEvent roundEvent)
    {
        string tooltip = "<color=#ffffff>" + roundEvent.eventName + "</color>\n\n";
        tooltip += "<color=#029919>" + roundEvent.eventDescription + "</color>\n\n";
        tooltip += "<color=#990282>" + "가격 : " + roundEvent.eventPrice + "</color>\t\t";
        tooltip += "<color=#990282>" + "필요 개발자 수 : " + roundEvent.eventRequiredDev + "</color>";
        return tooltip;
    }

    void SetRandomRoundEventID()
    {
        RandomRoundEventID = new List<int>();

        for (int i = 0; i < database.roundEventDatabase.Count; i++)
        {
            RandomRoundEventID.Add(i);
        }
    }
}
