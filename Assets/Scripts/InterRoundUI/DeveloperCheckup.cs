using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeveloperCheckup : MonoBehaviour {

    public GameObject postListTemplate;
    public static DeveloperCheckup devChkup;

    private GameObject developerPanel;
    private GameObject developerScrollPanel;
    private GameObject developerCloseButton;
    private GameObject developerBackButton;
    private Text developerStatusText;
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
        developerCloseButton = GameObject.Find("DeveloperCloseButton");
        developerBackButton = GameObject.Find("DeveloperBackButton");
        developerStatusText = GameObject.Find("DeveloperStatus").GetComponentInChildren<Text>();
        developerScrollPanelrect = developerScrollPanel.GetComponent<RectTransform>();
        developerPanel.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        developerPanel.SetActive(false);
        MakeDeveloperList();
        RefreshPostButtons2();
        database.CalculateCost();
        developerStatusText.text = "남은 돈 : " + GameManager.gm.Money() + "\t\t다음 라운드의 예상 월급 : " + (database.salaryCost * (GameManager.gm.basicTime - 10));
	}

    void MakeDeveloperList()
    {
        SetListSize(developerScrollPanelrect);
        foreach (Post post in database.postDatabase)
        {
            GameObject newPost = Instantiate(postListTemplate, new Vector3(0f, (developerScrollPanelrect.rect.height / 2) - 120f * post.postID - 20f, 0f), Quaternion.identity) as GameObject;
            newPost.name = post.postFuncName;

            /* 다른 children이 추가되면 아래 코드에서 에러가 발생할 수도? */
            //newPost.GetComponentInChildren<Image>().sprite = Sprite.Create(post.postImage, new Rect(0, 0, imageIconSize, imageIconSize), new Vector2(0f, 0f));
            // 이미지는 리소스가 생기면 사용할 예정
            newPost.transform.SetParent(developerScrollPanel.transform, false);

            newPost.GetComponentInChildren<DeveloperHireButton>().SetPost(post);
            newPost.GetComponentInChildren<DeveloperMoveFromButton>().SetPost(post);
            newPost.GetComponentInChildren<DeveloperFireButton>().SetPost(post);
            newPost.GetComponentInChildren<DeveloperMoveToButton>().SetPost(post);

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
        rect.sizeDelta = new Vector2(rect.rect.width, database.postDatabase.Count * 120f + 70f);
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
                tooltip += "<color=#990282>" + "개발자 1명당 월급 : " + (post.postSalary * (GameManager.gm.basicTime - 10)) + "</color>";
                p.GetComponentInChildren<Text>().text = tooltip;
            }
        }
        developerStatusText.text = "남은 돈 : " + GameManager.gm.Money() + "\t\t\t다음 라운드의 예상 월급 : " + (database.salaryCost * (GameManager.gm.basicTime - 10));
    }

    /// <summary>
    /// 부서 이동(MoveFrom) 버튼을 눌렀을 때 호출되어 DeveloperPanel의 UI를 변경해 주는 함수
    /// </summary>
    public void RefreshPostButtons1()
    {
        developerCloseButton.SetActive(false);
        developerBackButton.SetActive(true);
        foreach (GameObject p in postList)
        {
            p.GetComponent<PostList>().ActiveMoveTo();
        }
        developerStatusText.text = database.temp.postName + "의 개발자 1명을 어느 부서로 옮길지 선택하세요.";
    }

    /// <summary>
    /// 여기로 이동(MoveTo) 버튼 또는 취소 버튼을 눌렀을 때 호출되어 DeveloperPanel의 UI를 변경해 주는 함수
    /// </summary>
    public void RefreshPostButtons2()
    {
        developerCloseButton.SetActive(true);
        developerBackButton.SetActive(false);
        foreach (GameObject p in postList)
        {
            p.GetComponent<PostList>().ActiveMoveFrom();
        }
        developerStatusText.text = "남은 돈 : " + GameManager.gm.Money() + "\t\t\t\t다음 라운드의 예상 월급 : " + (database.salaryCost * (GameManager.gm.basicTime - 10));
    }

    public void CancelMove()
    {
        database.temp = null;
        RefreshPostButtons2();
    }
}
