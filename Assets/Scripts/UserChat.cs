using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserChat : MonoBehaviour {

    private GameManager gm; //많이 쓸것같아서 만들어둠

    public GameObject canvas;
    public GameObject ChatText;

    void Start ()
    {
        gm = GameManager.gm;
        _ChatText = ChatText;
        _canvas = canvas;

        gm.UserChat += NoDaram;
        gm.UserChat += DaramNumber;
    }

    private static GameObject _canvas;
    private static GameObject _ChatText;
    public static GameObject CreateChat(string text, float lifetime)
    {
        Vector3 pos = new Vector3(Random.Range(100, 600), Random.Range(200, 500), 0);   //화면 해상도에 따라 바꿀 것

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
        if (NDCool < Time.time)
            if (Daram.All.Count == 0)
            {
                if (Random.Range(0, 2) == 0)
                    CreateChat("다람쥐가 하나도 없잖아!!", 4);
                else
                    CreateChat("넥슨은 다람쥐를 뿌려라!", 3);
                NDCool = Time.time + Random.Range(2, 4);
            }
    }

    private float DNCool = 0;
    void DaramNumber()
    {
        if (DNCool < Time.time)
        {
            int targetnumber = 10 + gm.Fame / 1000;

            // f(x) = 5 - | y - x |
            int famediff = 5 - Mathf.Abs(Daram.All.Count - targetnumber);

            if (famediff == 0)
                return;
            if (famediff > 0)
                CreateChat("이 게임 할만하구만.", 3);
            else if (Daram.All.Count < targetnumber)
                switch (Random.Range(0, 2))
                {
                    case 0:
                        CreateChat("사람이 다람쥐보다 많은듯..", 3);
                        break;
                    case 1:
                        CreateChat("일해라 GM!!", 3);
                        break;
                }
            else
                switch (Random.Range(0, 2))
                {
                    case 0:
                        CreateChat("다람쥐에 깔려죽겠다 ㅠㅠ", 3);
                        break;
                    case 1:
                        CreateChat("뭉쳐다닐 파티 구합니다 (3/8)", 3);
                        break;
                }

            DNCool = Time.time + Random.Range(5, 10);
        }
    }
}
