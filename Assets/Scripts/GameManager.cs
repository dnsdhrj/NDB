using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    public GameObject resultScene; 

    public int Money = 0;
    [HideInInspector] public int EarnedMoney = 0;
    [HideInInspector] public int Fame = 0;
    [HideInInspector] public int[] UserCount;
    [HideInInspector] public int StageLevel = 1;
    [HideInInspector] public int TimeLeft;
    [HideInInspector] public bool IsPaused;
    //                public int UserAllCount();

    public float FieldCenterX=-2f;
    public float FieldCenterY=1.75f;
    public float FieldWidth = 12f;
    public float FieldHeight = 8.5f;

    // 어떤 이벤트가 발생하여 지속 효과를 넣어줄 때 사용
    // 항목에 맞게 분류해서 넣어주세요
    public delegate void Simulation();
    public event Simulation DaramDeath;     // 매 프레임마다 호출
    public event Simulation FameChange;     // 매 프레임마다 호출
    public event Simulation EventCheck;     // 매 프레임마다 호출
    public event Simulation UserChat;
    public event Simulation UserChange;     // 1초에 한번 호출
    public event Simulation StageEnd;       // 매 프레임마다 호출

    // public 함수들
    //public Vector2 RandomPosition();


    void Awake()
    {
        gm = this;
        resultScene = GameObject.Find("ResultScene");
        resultScene.SetActive(false);
        //UserCount 모두 0으로 초기화
        UserCount = Enumerable.Repeat(0, User.Count).ToArray();
        UserCount[User.level1] = 1000;
        IsPaused = false;

        //테스트용이고 나중에 삭제바람
        DaramDeath += DaramDeath_test;
        FameChange += FameDaram1;
        UserChange += UserLevel1;

        FameChange += CheckFameZero;
        StageEnd += StageEndCheck;      // 새 스테이지를 시작할 때마다 이것을 써 줘야 함.
        

        Random.seed = (int)Time.time;
    }

    void Start()
    {
        StartCoroutine("UserChangeCall");
        StartCoroutine("MoneyGainByFame");
    }

    void Update()
    {
        if (EventCheck != null)
            EventCheck();
        if (FameChange != null)
            FameChange();
        if (DaramDeath != null)
            DaramDeath();
        if (UserChat != null)
            UserChat();
        if (StageEnd != null)
            StageEnd();

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

    public int UserAllCount()
    {
        int sum = 0;
        foreach (int user in UserCount)
            sum += user;
        return sum;
    }

    IEnumerator UserChangeCall()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if(UserChange != null)
                UserChange();
            CheckUserZero();    //항상 마지막에 호출되게 함
        }
    }

#region 게임 시뮬레이션 관련 함수입니다

    //                      //
    //  다람쥐가 죽는 정도   //
    //                      //

    void DaramDeath_test()
    {
        if (IsPaused) return;
        int count = 0;

        // 어떤 요인에 의해
        if (Daram.All.Count != 0)
            count = (int)(UserCount[User.level1] / 1000.0f + UserCount[User.level2] / 100.0f);

        //다람쥐에게 피해를 입힌다
        for (int i = 0; i < count; i++)
        {
            int all = Daram.All.Count;
            if (all != 0)
                Daram.All[Random.Range(0, all)].HP -= 1;
        }
    }

    //                      //
    //        인기도        //
    //                      //

    void FameDaram1()
    {
        if (IsPaused) return;
        int a = 10 + Fame / 1000;   //다람쥐의 적정 숫자
        int x = Daram.All.Count;

        // y = k(x - a)^2 + max   (y >= min)
        Fame += (int) Mathf.Max(-5.0f, -0.2f * (x - a) * (x - a) + 5); 
    }

    //lv2 다람쥐가 해금되면 실행됨
    public void FameDaram2()
    {
        if (IsPaused) return;
        int a = 5 + UserCount[User.level2] / 100 + UserCount[User.level1] / 2000;   //다람쥐의 적정 숫자
        int x = Daram.FindByType("Basic", 2);

        // y = k(x - a)^2 + max   (y >= min)
        Fame += (int)Mathf.Max(-3.0f, -0.2f * (x - a) * (x - a) + 2);
    }

    //                      //
    //         유저         //
    //                      //

    private int PrevFame = 0;
    void UserLevel1()
    {
        if (IsPaused) return;
        int FameDelta = Fame - PrevFame;

        if(FameDelta > 0)
            UserCount[User.level1] += (int) (10 * Mathf.Log(Fame + 1));     // y = k * log(x + 1)
        else
            UserCount[User.level1] -= (int) (10 * Mathf.Log((-1)*FameDelta + 1));   //인기도가 감소중이면 적당히 줄어들게 함

        PrevFame = Fame;
    }

    //lv2 다람쥐가 해금되면 실행됨
    public void UserLevel2()
    {
        if (IsPaused) return;
        int LevelUp = 10 + UserCount[User.level1] / 1000;
        UserCount[User.level1] -= LevelUp;
        UserCount[User.level2] += LevelUp;  // 일단 level2유저는 감소하지 않는걸로
    }

    //                      //
    //       기타 함수      //
    //                      //

    void CheckFameZero()
    {
        if (Fame <= 0)
        {

            // 뭔가를 한다

            Fame = 0;
        }
    }

    void CheckUserZero()
    {
        for(int i = 0; i < User.Count; i++)
            if (UserCount[i] < 0)
                UserCount[i] = 0;      
    }


#endregion


    /* Functions about Money */

    //인기도에 의해 정기적으로 버는 소득
    IEnumerator MoneyGainByFame()
    {
        while (true) {
            if (!IsPaused)
            {
                EarnedMoney += Mathf.Max(0, (int)Mathf.Log(Fame, 2f));
            }
            yield return new WaitForSeconds(1f);
        }
    }

    //현재 남은 돈


    //다람쥐 생성에서 소모되는 비용
    void DaramLoss(int DaramCost)
    {
        Money -= DaramCost;
    }

    //이벤트(홍보, 긴급점검 등)에 의해 발생하는 돈의 증감
    //둘 다 + 이므로 parameter에 양수/음수를 잘 선정해서 넣어줘야 함
    void MoneyGainByEvent(int EventGain, int EventCost)
    {
        if (IsPaused) return;
        EarnedMoney += EventGain;
        Money += EventCost;
    }

    //스테이지가 끝날 때 GameManager에 결과 저장
    void MoneyUpdate()
    {
        Money = (Money + EarnedMoney);
        EarnedMoney = 0;
    }



    public Vector2 RandomPosition()
    {
        Vector2 randompos;
        randompos.x = Random.Range(FieldCenterX - FieldWidth / 2f, FieldCenterX + FieldWidth / 2f);
        randompos.y = Random.Range(FieldCenterY - FieldHeight / 2f, FieldCenterY + FieldHeight / 2f);
        return randompos;
    }

    public void SetStageTime() {
        int BasicTime = 50;
        TimeLeft = BasicTime + StageLevel * 10;
    }

    private void StageEndCheck() {
        if (TimeLeft <= 0) {
            StageEnd -= StageEndCheck;
//          print("stageEnded");
            resultScene.SetActive(true);
            Pause();    // 결과창이 뜰 때 일시정지 실행
        }
    }


    // Pause();를 한 번 사용하면 게임 일시정지, 두 번 사용하면 일시정지 해제
    // 일시정지 효과를 받아야 하는 void 함수의 맨 위에 "if (IsPaused) return;"을 추가하면 됨.
    // 시간을 멈추는 방법 대신 함수를 멈추는 방법으로 구현
    // 현재 인기도, 유저, 돈 관련 함수와 유저 채팅, 다람쥐 생성 버튼에 일시정지 효과 적용
    public void Pause()
    {
//      Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        IsPaused = IsPaused == true ? false : true;
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
