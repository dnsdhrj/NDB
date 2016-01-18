using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserChat : MonoBehaviour {

    private GameManager gm; //많이 쓸것같아서 만들어둠

    public static UserChat uc;
    public GameObject canvas;
    public GameObject ChatText;


    void Start ()
    {
        uc = this;
        gm = GameManager.gm;
        _ChatText = ChatText;
        _canvas = canvas;

        gm.UserChat += NoDaram;
        gm.UserChat += DaramNumber;
        gm.UserChat += UserCount;
    }

    private static GameObject _canvas;
    private static GameObject _ChatText;
    public static GameObject CreateChat(string text, float lifetime)
    {
        Vector3 pos = new Vector3(Random.Range(100, 500), Random.Range(200, 500), 0);   //화면 해상도에 따라 바꿀 것

        GameObject obj = (GameObject) Instantiate(_ChatText, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(_canvas.transform);
        obj.GetComponent<RectTransform>().position = pos;
        obj.GetComponent<Text>().text = text;
        obj.GetComponent<SelfDestroyScript>().ChangeFate(lifetime);
        return obj;
    }

    private float NDCool = 0;
    void NoDaram()
    {
        if (NDCool < gm.time)
            if (Daram.All.Count == 0)
            {
                if (Random.Range(0, 2) == 0)
                    CreateChat("다람쥐가 하나도 없잖아!!", 4);
                else
                    CreateChat("넥슨은 다람쥐를 뿌려라!", 2);
                NDCool = gm.time + Random.Range(2, 4);
            }
    }

    private float DNCool = 0;
    void DaramNumber()
    {
        if (DNCool < gm.time)
        {
            int a = 10 + gm.Fame / 1000;   //다람쥐의 적정 숫자
            int x = Daram.All.Count;

            // y = k(x - a)^2 + max   (y >= min)
            int famediff = (int)Mathf.Max(-5.0f, -0.2f * (x - a) * (x - a) + 5);

            if (famediff == 0)
                return;
            if (famediff > 0)
                switch(Random.Range(0,3))
                {
                    case 0:
                        CreateChat("이 게임 할만하구만.", 3);
                        break;
                    case 1:
                        CreateChat("좋아 좋아.", 3);
                        break;
                    case 2:
                        CreateChat("열정적인 GM!", 3);
                        break;
                }
            else if (x < a)
                switch (Random.Range(0, 4))
                {
                    case 0:
                        CreateChat("사람이 다람쥐보다 많은듯..", 3);
                        break;
                    case 1:
                        CreateChat("일해라 GM!!", 3);
                        break;
                    case 2:
                        CreateChat("다람쥐 멸종위기....", 3);
                        break;
                    case 3:
                        CreateChat("넥슨은 다람쥐를 뿌려라!", 2);
                        break;
                }
            else
                switch (Random.Range(0, 3))
                {
                    case 0:
                        CreateChat("다람쥐에 깔려죽겠다 ㅠㅠ", 3);
                        break;
                    case 1:
                        CreateChat("이걸 다 언제잡아..", 3);
                        break;
                    case 2:
                        CreateChat("공기반 다람쥐반이네..", 3);
                        break;
                }

            DNCool = gm.time + Random.Range(5, 10);
        }
    }

    private float DN2Cool = 0;
    public void Daram2Number()
    {
        if (DN2Cool < gm.time)
        {
            int a = 5 + gm.UserCount[User.level2] / 100 + gm.UserCount[User.level1] / 2000;   //다람쥐의 적정 숫자
            int x = Daram.FindByType("Basic", 2);

            // y = k(x - a)^2 + max   (y >= min)
            int famediff = (int)Mathf.Max(-3.0f, -0.2f * (x - a) * (x - a) + 2);

            if (famediff == 0)
                return;
            if (famediff > 0)
                return;
            else if (x < a)
                switch (Random.Range(0, 3))
                {
                    case 0:
                        CreateChat("아직도 Lv1짜리 다람쥐뿐인가..", 3);
                        break;
                    case 1:
                        CreateChat("고렙 다람쥐가 있기는 한거야?", 3);
                        break;
                    case 2:
                        CreateChat("한방짜리 다람쥐는 너무 시시하다구!!", 3);
                        break;
                }
            else
                switch (Random.Range(0, 3))
                {
                    case 0:
                        CreateChat("초보자 배려좀 ㅠㅠ", 3);
                        break;
                    case 1:
                        CreateChat("고렙 다람쥐 잡을 파티 구합니다 (5/8)", 3);
                        break;
                    case 2:
                        CreateChat("여기 초보자 사냥터 아닌가여;;;;", 3);
                        break;
                }

            DN2Cool = gm.time + Random.Range(3, 5);
        }
    }

    private float UCCool = 15;
    void UserCount()
    {
        if (DN2Cool < gm.time)
        {
            CreateChat("지금 유저 수가 " + ((gm.UserAllCount() + 500) / 1000) * 1000 + "명쯤은 되는듯.", 4);    //천명 단위로 끊음(1000, 2000...)
            DN2Cool = gm.time + Random.Range(20, 40);
        }
    }
}
