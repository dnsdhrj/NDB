using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    private GameObject resultScene; 

    public int Money = 0;
    public int UserLimit;
    [HideInInspector] public float time = 0;    // 일시정지를 보정한 시간
    [HideInInspector] public int EarnedMoney = 0;
    [HideInInspector] public int Fame = 0;
    [HideInInspector] public int[] UserCount;
    [HideInInspector] public Quadric[] DaramFunction;   // 적정 다람쥐 계산하는 함수
    [HideInInspector] public int StageLevel = 1;
    [HideInInspector] public int TimeLeft;
    [HideInInspector] public string CurrentStageScene;
    [HideInInspector] public bool IsPaused = false;
    //                public bool IsInterRound;         // InterRound때 일시정지는 되어 있음


    public float FieldCenterX;
    public float FieldCenterY;
    public float FieldWidth;
    public float FieldHeight;

    public GameObject StartScene;

    // 어떤 이벤트가 발생하여 지속 효과를 넣어줄 때 사용
    // 항목에 맞게 분류해서 넣어주세요
    public delegate void Simulation();
    public event Simulation DaramDeath;     // 매 프레임마다 호출
    public event Simulation FameChange;     // 매 프레임마다 호출
    public event Simulation EventCheck;     // 매 프레임마다 호출
    public event Simulation UserChat;       // 매 프레임마다 호출
    public event Simulation UserChange;     // 1초에 한번 호출

    // public 함수들
    //public Vector2 RandomPosition();
    //public void pause(bool pause);
    //public int UserAllCount();

    private static bool GMCreated = false;
    void Awake()
    {
        if (GMCreated == true)  // GM 중복생성 방지
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 60;
        DontDestroyOnLoad(this);    // 씬이 넘어가도 파괴되지 않음

        gm = this;
        CurrentStageScene = SceneManager.GetActiveScene().name;

        //UserCount 초기화
        UserCount = Enumerable.Repeat(0, User.Count).ToArray();
        UserCount[User.level1] = 1000;

        DaramFunction = new Quadric[User.Count];
        for (int i = 0; i < User.Count; i++)
            DaramFunction[i] = new Quadric();



        //테스트용이고 나중에 삭제바람
        DaramDeath += DaramDeath_test;

        FameChange += FameDaram1;
        UserChange += UserLevel1;
        FameChange += CheckFameZero;
        

        Random.seed = (int)Time.time;
    }

    void Start()
    {
        if (GMCreated == true)  // GM 중복생성 방지
            return;
        GMCreated = true;

        OnLevelWasLoaded(0);    // Start 대신 저 안에 써주세요

        Random.seed = (int)Time.time;
    }

    void OnLevelWasLoaded(int level)
    {
        // 라운드 시작시마다 실행
        if (IsInterRound == false)
        {
            gm.time = Time.time;
            SetRoundTime();

            resultScene = GameObject.Find("ResultScene");

            StartCoroutine("UserChangeCall");
            StartCoroutine("MoneyGainByFame");

            if (StartScene != null)
                Instantiate(StartScene);
        }
    }

    void Update()
    {
        UpdateGameTime();

        if (!IsPaused)
        {
            if (EventCheck != null)
                EventCheck();
            if (FameChange != null)
                FameChange();
            if (DaramDeath != null)
                DaramDeath();
            if (UserChat != null)
                UserChat();
        }
        if(!IsInterRound)
            RoundEndCheck();

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
            while (IsPaused)
                yield return null;
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
        Quadric q = DaramFunction[User.level1];
        q.k = 0.2f;
        q.x = Daram.All.Count;
        q.a = 10 + Fame / 1000;
        q.max = 5;
        q.min = -5;

        Fame += (int) q.value;
    }

    //lv2 다람쥐가 해금되면 실행됨
    public void FameDaram2()
    {
        Quadric q = DaramFunction[User.level2];
        q.k = 0.2f;
        q.x = Daram.FindByType("Basic", 2);
        q.a = 5 + UserCount[User.level2] / 100 + UserCount[User.level1] / 2000;
        q.max = 2;
        q.min = -3;

        Fame += (int) q.value;
    }

    //                      //
    //         유저         //
    //                      //

    private int PrevFame = 0;
    void UserLevel1()
    {
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
                EarnedMoney += 2 * Mathf.Max(0, (int)Mathf.Log(Fame, 2f));
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

    public void SetRoundTime() {
        int BasicTime = 5;
        TimeLeft = BasicTime; //+ StageLevel * 10;
    }

    private void RoundEndCheck() {
        if (TimeLeft <= -1) {   // 0으로 하면 마지막 1초가 보여지지 않아서 -1로 수정
            //print("stageEnded");
            resultScene.SetActive(true);
            resultScene.GetComponent<ResultScene>().isEnabled = true;
            Pause(true);    // 결과창이 뜰 때 일시정지 실행
        }
    }


    // Pause();를 한 번 사용하면 게임 일시정지, 두 번 사용하면 일시정지 해제 -> 간혹 충돌하는 경우가 있어서 인자 넘겨주는 것으로 변경
    // 일시정지 효과를 받아야 하는 void 함수의 맨 위에 "if (IsPaused) return;"을 추가하면 됨.
    // 시간을 멈추는 방법 대신 함수를 멈추는 방법으로 구현
    // 현재 인기도, 유저, 돈 관련 함수와 유저 채팅, 다람쥐 생성 버튼에 일시정지 효과 적용
    public void Pause(bool pause)
    {
        //Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        IsPaused = pause;
    }


    // 일시정지 시간을 빼서 시간을 나타냄
    private bool lasting = false;
    private float PauseStart;
    private float PausedTime = 0;
    void UpdateGameTime()
    {
        if (gm.IsPaused)
        {
            if (!lasting)
            {
                lasting = true;
                PauseStart = Time.time;
            }
        }
        else
        {
            if (lasting)
            {
                lasting = false;
                PausedTime += Time.time - PauseStart;
            }
            time = Time.time - PausedTime;
        }
    }

    public bool IsInterRound
    {
        get
        {
            return SceneManager.GetActiveScene().name == CurrentStageScene ? false : true;
        }
    }
}



// C#의 enum은 array index로 못 쓴다고 합니다 ㅠㅠ
// 이 방법 외에 좋은 방법 찾으면 수정좀
public static class User
{
    // User의 변수 개수
    public static int Count = 2;

    // 이 순서대로 게이지가 정렬됨
    public static int level1 = 0;
    public static int level2 = 1;
}
