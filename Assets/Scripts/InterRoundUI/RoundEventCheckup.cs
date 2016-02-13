using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RoundEventCheckup : MonoBehaviour {


    public GameObject eventPanel;
    public GameObject roundEventListTemplate;

    private RoundEventDatabase database;

    private int imageIconSize = 256;

    void Start ()
    {
        database = GameManager.gm.GetComponentInChildren<RoundEventDatabase>();
        eventPanel = GameObject.Find("EventPanel");
        eventPanel.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        eventPanel.SetActive(false);
    }

    public void ShowRoundEvents()
    {

        eventPanel.SetActive(true);

        foreach (RoundEvent roundEvent in database.roundEventDatabase)
        {
            List<GameObject> eventList = new List<GameObject>();

            GameObject newEvent = Instantiate(roundEventListTemplate, new Vector3(0f, -120f * roundEvent.eventID, 0f), Quaternion.identity) as GameObject;
            newEvent.name = roundEvent.eventName;

            /* 다른 children이 추가되면 아래 코드에서 에러가 발생할 수도? */
            newEvent.GetComponentInChildren<Image>().sprite = Sprite.Create(roundEvent.eventImage, new Rect(0, 0, imageIconSize, imageIconSize), new Vector2(0f, 0f));
            newEvent.transform.SetParent(GameObject.Find("EventPanel").transform, false);
            newEvent.GetComponentInChildren<Text>().text = CreateEventTooltip(roundEvent);
            //newEvent.GetComponentInChildren<ItemBuyButton>().SetEvent(roundEvent);

            eventList.Add(newEvent);

        }

    }

    public string CreateEventTooltip(RoundEvent roundEvent)
    {
        string tooltip = "<color=#ffffff>" + roundEvent.eventName + "</color>\n\n";
        tooltip += "<color=#029919>" + roundEvent.eventDescription + "</color>\n\n";
        tooltip += "<color=#990282>" + "가격 : " + roundEvent.eventPrice + "</color>";
        tooltip += "<color=#990282>" + "필요 개발자 수 : " + roundEvent.eventRequiredDev + "</color>";
        return tooltip;
    }
}
