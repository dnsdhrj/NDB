using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserChat : MonoBehaviour {

    private GameManager gm; //많이 쓸것같아서 만들어둠

    public static UserChat uc;
    public GameObject ChatText;


    void Start ()
    {
        uc = this;
        gm = GameManager.gm;
        _ChatText = ChatText;

        gm.UserChat += NoDaram;
        gm.UserChat += DaramNumber;
        gm.UserChat += UserCount;
    }

    private static GameObject _Canvas = null;
    private static GameObject _ChatText;
    public static GameObject CreateChat(string text, float lifetime)
    {
        Vector3 pos = new Vector3(Random.Range(100, 500), Random.Range(300, 500), 0);   //화면 해상도에 따라 바꿀 것

        if (_Canvas == null)
            _Canvas = GameObject.Find("Canvas");
        GameObject obj = (GameObject) Instantiate(_ChatText, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(_Canvas.transform);
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
            Quadric q = gm.DaramFunction[User.level1];

            if (q.value == 0)
                return;
            if (q.value > 0)
                switch(Random.Range(0,4))
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
                    case 3:
                        CreateChat("이게 요즘 흥한다는 그 게임인가요?", 3);
                        break;
                }
            else if (q.diff < 0)
                switch (Random.Range(0, 5))
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
                    case 4:
                        CreateChat("다람쥐가 있어야 퀘스트를 하지...", 3);
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
            Quadric q = gm.DaramFunction[User.level2];

            if (q.value == 0)
                return;
            if (q.value > 0)
                return;
            else if (q.diff < 0)
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
        if (UCCool < gm.time)
        {
            CreateChat("지금 유저 수가 " + ((gm.UserAllCount() + 500) / 1000) * 1000 + "명쯤은 되는듯.", 4);    //천명 단위로 끊음(1000, 2000...)
            UCCool = gm.time + Random.Range(15, 30);
        }
    }

    private float ULECool = 0;
    private int ULECounter = 0;
    public void UserLimitExcess()
    {
        if (ULECool == 0)
        {
            if (!(gm.isPaused))
            {
                ULECool = gm.time + 3.0f;
                CreateChat("헐 뭐임?", 5);
                CreateChat("팅김 ㄷㄷ", 5);
                CreateChat("뭐야, 또 터진거야?", 5);
            }
        }
        else if (ULECool < gm.time)
        {
            if (ULECounter < 6)
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        CreateChat("겨우겨우 접속했네 ㅠ", 3);
                        break;
                    case 1:
                        CreateChat("또 긴급점검 할까봐 무섭..", 3);
                        break;
                    case 2:
                        CreateChat("GM: 여러분의 쾌적한 플레이를\n        위해 최선을 다하겠습니다", 4);
                        break;
                    case 3:
                        CreateChat("흥겜 서버증설좀 해주세요!!", 3);
                        break;
                }

                ULECool = gm.time + Random.Range(1, 3);
                ULECounter += Random.Range(2, 5);
            }
            else
                gm.UserChat -= UserLimitExcess;
        }

    }
}
