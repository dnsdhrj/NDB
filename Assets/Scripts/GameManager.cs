using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    [HideInInspector] public int Money = 0;
    [HideInInspector] public int Fame = 0;
    [HideInInspector] public int[] UserCount;


    void Awake()
    {
        gm = this;

        //UserCount 모두 0으로 초기화
        UserCount = Enumerable.Repeat(0, User.Count).ToArray();

        Random.seed = (int)Time.time;
    }

    void Update()
    {
        DaramDeath();

        if (Input.GetKeyDown("f2")) //디버그용
            DebugFunc();
        if (Input.GetKeyDown("f3")) //각종 변수 상태 출력
            DebugStatFunc();
    }

    void DebugFunc()
    {
        
    }

    void DebugStatFunc()
    {
        print("Level 1 : " + UserCount[User.level1]);
        print("Level 2 : " + UserCount[User.level2]);
        print("다람쥐 개수 : " + Daram.All.Count);
    }

    void DaramDeath()
    {
        int count = 0;

        // 어떤 요인에 의해
        if(Daram.All.Count != 0)
            count = 1;

        //다람쥐에게 피해를 입힌다
        for (int i = 0; i < count; i++)
            Daram.All[Random.Range(0, Daram.All.Count)].HP -= 1;
    }
}

// C#의 enum은 array index로 못 쓴다고 합니다 ㅠㅠ
// 이 방법 외에 좋은 방법 찾으면 수정좀
public static class User
{
    // User의 변수 개수
    public static int Count = 2;

    public static int level1 = 0;
    public static int level2 = 1;
}
