using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeveloperCheckup : MonoBehaviour {

    public GameObject postListTemplate;
    public static DeveloperCheckup devChkup;

    //private Text hireButtonText;
    //private Text fireButtonText;
    private GameObject developerPanel;
    private GameObject developerScrollPanel;
    private Developer database;
    private RectTransform developerScrollPanelrect;
    private int imageIconSize = 256;

    private List<GameObject> postList = new List<GameObject>();

    void Start()
    {
        devChkup = this;
        database = GameManager.gm.GetComponent<Developer>();
        developerPanel = GameObject.Find("DeveloperPanel");
        developerScrollPanel = GameObject.Find("DeveloperScrollPanel");
        developerScrollPanelrect = developerScrollPanel.GetComponent<RectTransform>();
        developerPanel.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        developerPanel.SetActive(false);
        MakeDeveloperList();

        //hireButtonText = GameObject.Find("HireButton").GetComponentInChildren<Text>();
        //fireButtonText = GameObject.Find("FireButton").GetComponentInChildren<Text>();
        //hireButtonText.text = "고용\n(" + Developer.dev.hireCost + ")";
        //fireButtonText.text = "해고\n(" + Developer.dev.fireCost + ")";
	}

    void MakeDeveloperList()
    {
        SetListSize(developerScrollPanelrect);
        foreach (Post post in database.postDatabase)
        {
            GameObject newPost = Instantiate(postListTemplate, new Vector3(0f, (developerScrollPanelrect.rect.height / 2) - 120f * post.postID, 0f), Quaternion.identity) as GameObject;
            newPost.name = post.postFuncName;

            /* 다른 children이 추가되면 아래 코드에서 에러가 발생할 수도? */
            //newPost.GetComponentInChildren<Image>().sprite = Sprite.Create(post.postImage, new Rect(0, 0, imageIconSize, imageIconSize), new Vector2(0f, 0f));
            // 이미지는 리소스가 생기면 사용할 예정
            newPost.transform.SetParent(developerScrollPanel.transform, false);

            newPost.GetComponentInChildren<DeveloperHireButton>().SetPost(post);
            newPost.GetComponentInChildren<DeveloperMoveFromButton>().SetPost(post);
            newPost.GetComponentInChildren<DeveloperFireButton>().SetPost(post);

            postList.Add(newPost);

        }
        RefreshPostTooltip();
    }
    public void ShowDevelopers()
    {
        //이유는 모르겠지만 처음에 위치 조정을 안해주면 스크롤바랑 이미지 표시가 이상해짐
        developerPanel.GetComponent<ScrollRect>().verticalScrollbar.value = 0;
        developerScrollPanelrect.localPosition = new Vector2(developerScrollPanelrect.localPosition.x, -developerScrollPanelrect.rect.height / 2);
        developerPanel.SetActive(true);
        RefreshPostTooltip();

    }

    void SetListSize(RectTransform rect)
    {
        rect.sizeDelta = new Vector2(rect.rect.width, database.postDatabase.Count * 120f);
    }

    public void RefreshPostTooltip()
    {
        foreach(GameObject p in postList)
        {
            Post post = database.FindPostByPostID(database.FindPostIDByName(p.name));
            if (post != null)
            {
                string tooltip = "<color=#ffffff>" + post.postName + "</color>\n\n";
                tooltip += "<color=#029919>" + post.postDescription + "</color>\n\n";
                tooltip += "<color=#990282>" + "투입된 개발자 수 : " + post.DeveloperInPost() + "</color>\t\t";
                tooltip += "<color=#990282>" + "개발자 1명당 월급 : " + post.postSalary + "</color>";
                p.GetComponentInChildren<Text>().text = tooltip;
            }
        }
    }


}
