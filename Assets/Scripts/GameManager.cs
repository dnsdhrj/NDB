using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    [HideInInspector] public int Money = 0;
    [HideInInspector] public int Fame = 0;
    [HideInInspector] public int[] UserCount;

    public float FieldWidth = 2f;
    public float FieldHeight = 2f;

    public delegate void DaramDeathEvent();
    public event DaramDeathEvent DaramDeath;
    public delegate void FameChangeEvent();
    public event FameChangeEvent FameChange;


    void Awake()
    {
        gm = this;

        //UserCount 모두 0으로 초기화
        UserCount = Enumerable.Repeat(0, User.Count).ToArray();

        //테스트용이고 나중에 삭제바람
        DaramDeath += DaramDeath_test;
        FameChange += FameChange_test;

        FameChange += CheckZero;

        Random.seed = (int)Time.time;
    }

    void Update()
    {
        if(DaramDeath != null)
            DaramDeath();
        if(FameChange != null)
            FameChange();

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

    void DaramDeath_test()
    {
        int count = 0;

        // 어떤 요인에 의해
        if(Daram.All.Count != 0)
            count = 1;

        //다람쥐에게 피해를 입힌다
        for (int i = 0; i < count; i++)
            Daram.All[Random.Range(0, Daram.All.Count)].HP -= 1;
    }

    void FameChange_test()
    {
        //다람쥐의 적정 숫자
        int targetnumber = 10;

        // f(x) = 5 - | y - x |
        Fame += 5 - System.Math.Abs(Daram.All.Count - targetnumber);
    }

    void CheckZero()
    {
        if (Fame <= 0)
        {

            // 뭔가를 한다

            Fame = 0;
        }
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
