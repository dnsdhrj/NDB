using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    public GameObject resultScene; 

    public int Money = 0;
    public int UserLimit;
    [HideInInspector] public float time = 0;    // 일시정지를 보정한 시간
    [HideInInspector] public int EarnedMoney = 0;
    [HideInInspector] public int Fame = 0;
    [HideInInspector] public int[] UserCount;
    [HideInInspector] public Quadric[] DaramFunction;   // 적정 다람쥐 계산하는 함수
    [HideInInspector] public int RoundCount = 0;
    [HideInInspector] public int TimeLeft = 1;
    [HideInInspector] public string CurrentStageScene;
    [HideInInspector] public bool IsPaused = false;
    [HideInInspector] public int Developers = 0;
    //                public bool IsInterRound;         // InterRound때 일시정지는 되어 있음, 대기시간 10초도 InterRound 취급


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
        //DaramDeath += DaramDeath_test;

        DaramDeath += DaramDeath1;
        DaramDeath += DaramDeath2;
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

            StartCoroutine("UserChangeCall");
            StartCoroutine("MoneyGainByFame");

            if (StartScene != null)
                Instantiate(StartScene);

            if (RoundCount != 1)    // 시작시에는 점기점검이 없습니다
            {
                LogText.WriteLog("");
                LogText.WriteLog( (RoundCount-1) + "번째 정기점검 끝.");
            }
            LogText.WriteLog("10초 후 유저 로그인이 활성화됩니다.");
        }
    }

    void Update()
    {
        UpdateGameTime();

        if (!IsPaused && !IsInterRound)
        {
            if (EventCheck != null)
                EventCheck();
            if (FameChange != null)
                FameChange();
            if (DaramDeath != null)
                DaramDeath();
            if (UserChat != null)
                UserChat();
            if (!IsInterRound)
                RoundEndCheck();
        }


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
            while (IsPaused || IsInterRound)
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

    // 다람쥐 하나를 일점사해서 사실적인 시뮬레이션을 만듬
    private Daram DaramBias = null;
    Daram RandomDaram()
    {
        if (DaramBias == null || Random.value < 0.01)   // 낮은 확률로 타겟 변경
            DaramBias = Daram.All[Random.Range(0, Daram.All.Count)];

        return DaramBias;
    }

    void DaramDeath1()  // 초보에 의한 데미지
    {
        int TotalDamage = (int)(UserCount[User.level1] / 1000.0f);

        while (Daram.All.Count != 0 && TotalDamage > 0)
        {
            float DamageEff = 1.0f;    // 다람쥐 종류별 데미지계수
            Daram d = RandomDaram();

            switch (d.Level)
            {
                case 1:
                    DamageEff = 1.0f;
                    break;
                case 2:
                    DamageEff = 0.7f;   // 초보는 고렙몹을 잘 못잡습니다
                    break;
            }
            float DamageDealt = Mathf.Max( Mathf.Min(TotalDamage * DamageEff, d.HP), 1);
            d.HP -= (int)DamageDealt;
            TotalDamage -= (int)(DamageDealt / DamageEff);
        }

    }

    void DaramDeath2()  // 중수에 의한 데미지
    {
        int TotalDamage = (int)(UserCount[User.level2] / 100.0f);

        while (Daram.All.Count != 0 && TotalDamage > 0)
        {
            float DamageEff = 1.0f;    // 다람쥐 종류별 데미지계수
            Daram d = RandomDaram();

            switch (d.Level)
            {
                case 1:
                    DamageEff = 0.5f;   // 흥미가 없어서 안잡습니다
                    break;
                case 2:
                    DamageEff = 1.0f;
                    break;
            }
            float DamageDealt = Mathf.Max( Mathf.Min(TotalDamage * DamageEff, d.HP), 1);
            d.HP -= (int)DamageDealt;
            TotalDamage -= (int)(DamageDealt / DamageEff);
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
            if (!IsPaused && !IsInterRound)
            {
                EarnedMoney += 2 * Mathf.Max(0, (int)Mathf.Log(Fame, 2f));
            }
            yield return new WaitForSeconds(1f);
        }
    }

    //현재 남은 돈


    //이벤트(홍보, 긴급점검 등)에 의해 발생하는 돈의 증감
    //둘 다 + 이므로 parameter에 양수/음수를 잘 선정해서 넣어줘야 함
    void MoneyGainByEvent(int EventGain, int EventCost)
    {
        EarnedMoney += EventGain;
        Money += EventCost;
    }

    



    public Vector2 RandomPosition()
    {
        Vector2 randompos;
        randompos.x = Random.Range(FieldCenterX - FieldWidth / 2f, FieldCenterX + FieldWidth / 2f);
        randompos.y = Random.Range(FieldCenterY - FieldHeight / 2f, FieldCenterY + FieldHeight / 2f);
        return randompos;
    }

    public void SetRoundTime() {
        int BasicTime = 40;
        TimeLeft = BasicTime; //+ RoundCount * 10;
        RoundCount++;
    }

    private void RoundEndCheck() {
        if (TimeLeft <= -1) {   // 0으로 하면 마지막 1초가 보여지지 않아서 -1로 수정
            //print("stageEnded");
            TimeLeft = 0;
            Instantiate(resultScene); // 결과창을 Instantiate하는 방법으로 변경
        }
    }


    // Pause(true);를 사용하면 게임 일시정지, Pause(false); 사용하면 일시정지 해제 (간혹 충돌하는 경우가 있어서 인자 넘겨주는 것으로 변경)
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
            if (TimeLeft >= 30) return true;
            return SceneManager.GetActiveScene().name == CurrentStageScene ? false : true;
        }
    }
}



// C#의 enum은 array index로 못 쓴다고 합니다 ㅠㅠ
// 이 방법 외에 좋은 방법 찾으면 수정좀
public static class User
{
    // User의 변수 개수
    public const int Count = 2;

    // 이 순서대로 게이지가 정렬됨
    public const int level1 = 0;
    public const int level2 = 1;
}
