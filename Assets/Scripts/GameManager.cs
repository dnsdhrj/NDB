using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager gm;
    private Developer dev;

    public GameObject resultScene; 

    public int money = 0;

    public int basicUserLimit;
    [HideInInspector] public int userLimit;
    [HideInInspector] public float time = 0;    // 일시정지를 보정한 시간
    [HideInInspector] public int earnedMoney = 0;
    [HideInInspector] public int fame = 0;
    [HideInInspector] public int[] userCount;
    [HideInInspector] public Quadric[] DaramFunction;   // 적정 다람쥐 계산하는 함수
    [HideInInspector] public int roundCount = 0;
    [HideInInspector] public int timeLeft = 1;
    [HideInInspector] public string currentStageScene;
    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool isRoundEventOn = false;
    //                public bool isInterRound;         // InterRound때 일시정지는 되어 있음, 대기시간 10초도 InterRound 취급


    public float fieldCenterX;
    public float fieldCenterY;
    public float fieldWidth;
    public float fieldHeight;

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

    private int basicTime = 1;
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
        dev = Developer.dev;
        currentStageScene = SceneManager.GetActiveScene().name;

        //UserCount 초기화
        userCount = Enumerable.Repeat(0, User.Count).ToArray();
        userCount[User.level1] = 1000;

        DaramFunction = new Quadric[User.Count];
        for (int i = 0; i < User.Count; i++)
            DaramFunction[i] = new Quadric();


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
        if (isInterRound == false)
        {
            gm.time = Time.time;
            SetRoundTime();

            StartCoroutine("UserChangeCall");
            StartCoroutine("MoneyGainByFame");

            if (StartScene != null)
                Instantiate(StartScene);

            if (roundCount != 1)    // 시작시에는 점기점검이 없습니다
            {
                LogText.WriteLog("");
                LogText.WriteLog( (roundCount-1) + "번째 정기점검 끝.");
            }
            LogText.WriteLog("10초 후 유저 로그인이 활성화됩니다.");
        }
    }

    void Update()
    {
        UpdateGameTime();

        if (!isPaused)
        {
            if (FameChange != null)
                FameChange();
            if (!isInterRound)
            {
                if (EventCheck != null)
                    EventCheck();
                if (DaramDeath != null)
                    DaramDeath();
                if (UserChat != null)
                    UserChat();
                RoundEndCheck();
            }
        }

        // 개발용 함수
        if (Input.GetKeyDown("f2")) //디버그용
            DebugFunc();
        if (Input.GetKeyDown("f3")) //각종 변수 상태 출력
            DebugStatFunc();
        if (Input.GetKeyDown("f4")) //InterRound로 즉시 이동
            timeLeft = 0;

    }

    void DebugFunc()
    {
       
    }

    void DebugStatFunc()
    {
        print("Level 1 : " + userCount[User.level1]);
        print("Level 2 : " + userCount[User.level2]);
        print("다람쥐 개수 : " + Daram.All.Count);
    }


    

    public int UserAllCount()
    {
        int sum = 0;
        foreach (int user in userCount)
            sum += user;
        return sum;
    }

    IEnumerator UserChangeCall()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            while (isPaused || isInterRound)
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
        int TotalDamage = (int)(userCount[User.level1] / 1000.0f);

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
        int TotalDamage = (int)(userCount[User.level2] / 100.0f);

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
        // IsInterRound가 true이면 인기도는 변하지 않아도 함수는 작동함
        Quadric q = DaramFunction[User.level1];
        q.k = 0.2f;
        q.x = Daram.All.Count;
        q.a = 10 + fame / 1000;
        q.max = 5;
        q.min = -5;

        if(!isInterRound) fame += (int) q.value;
    }

    //lv2 다람쥐가 해금되면 실행됨
    public void FameDaram2()
    {
        // IsInterRound가 true이면 인기도는 변하지 않아도 함수는 작동함
        Quadric q = DaramFunction[User.level2];
        q.k = 0.2f;
        q.x = Daram.FindByType("Basic", 2);
        q.a = 5 + userCount[User.level2] / 100 + userCount[User.level1] / 2000;
        q.max = 2;
        q.min = -3;

        if(!isInterRound) fame += (int) q.value;
    }

    //                      //
    //         유저         //
    //                      //

    private int PrevFame = 0;
    void UserLevel1()
    {
        int FameDelta = fame - PrevFame;

        if(FameDelta > 0)
            userCount[User.level1] += (int)((10 + 2 * dev.developerCount[Developer.dev.FindPostIDByName("Publicity")]) * Mathf.Log(fame + 1));     // y = k * log(x + 1)
        else
            userCount[User.level1] -= (int) (10 * Mathf.Log((-1)*FameDelta + 1));   // 인기도가 감소중이면 적당히 줄어들게 함

        PrevFame = fame;
    }

    //lv2 다람쥐가 해금되면 실행됨
    public void UserLevel2()
    {
        int LevelUp = 10 + userCount[User.level1] / 1000;
        userCount[User.level1] -= LevelUp;
        userCount[User.level2] += LevelUp;  // 일단 level2유저는 감소하지 않는걸로
    }

    //                      //
    //       기타 함수      //
    //                      //

    void CheckFameZero()
    {
        if (fame <= 0)
        {

            // 뭔가를 한다

            fame = 0;
        }
    }

    void CheckUserZero()
    {
        for(int i = 0; i < User.Count; i++)
            if (userCount[i] < 0)
                userCount[i] = 0;      
    }


#endregion


    /* Functions about Money */

    //인기도에 의해 정기적으로 버는 소득
    IEnumerator MoneyGainByFame()
    {
        while (true) {
            if (!isPaused && !isInterRound)
            {
                earnedMoney += 2 * Mathf.Max(0, (int)Mathf.Log(fame, 2f));
            }
            yield return new WaitForSeconds(1f);
        }
    }

    //현재 남은 돈


    //이벤트(홍보, 긴급점검 등)에 의해 발생하는 돈의 증감
    //둘 다 + 이므로 parameter에 양수/음수를 잘 선정해서 넣어줘야 함
    void MoneyGainByEvent(int EventGain, int EventCost)
    {
        earnedMoney += EventGain;
        money += EventCost;
    }

    



    public Vector2 RandomPosition()
    {
        Vector2 randompos;
        randompos.x = Random.Range(fieldCenterX - fieldWidth / 2f, fieldCenterX + fieldWidth / 2f);
        randompos.y = Random.Range(fieldCenterY - fieldHeight / 2f, fieldCenterY + fieldHeight / 2f);
        return randompos;
    }

    public void SetRoundTime() {
        timeLeft = basicTime; //+ RoundCount * 10;
        roundCount++;
    }

    private void RoundEndCheck() {
        if (timeLeft <= -1) {   // 0으로 하면 마지막 1초가 보여지지 않아서 -1로 수정
            //print("stageEnded");
            timeLeft = 0;

            //라운드 이벤트 제거를 위해 한번 더 확인
            isRoundEventOn = false;
            if (EventCheck != null)
                EventCheck();

            Instantiate(resultScene); // 결과창을 Instantiate하는 방법으로 변경
            // 방법을 변경한 이유는 결과창이 맨 위에 뜨도록 하기 위해서임.
        }
    }


    // Pause(true);를 사용하면 게임 일시정지, Pause(false); 사용하면 일시정지 해제 (간혹 충돌하는 경우가 있어서 인자 넘겨주는 것으로 변경)
    // 일시정지 효과를 받아야 하는 void 함수의 맨 위에 "if (IsPaused) return;"을 추가하면 됨.
    // 시간을 멈추는 방법 대신 함수를 멈추는 방법으로 구현
    // 현재 인기도, 유저, 돈 관련 함수와 유저 채팅, 다람쥐 생성 버튼에 일시정지 효과 적용
    public void Pause(bool pause)
    {
        //Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        isPaused = pause;
    }


    // 일시정지 시간을 빼서 시간을 나타냄
    private bool lasting = false;
    private float PauseStart;
    private float PausedTime = 0;
    void UpdateGameTime()
    {
        if (gm.isPaused)
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

    public bool isInterRound
    {
        get
        {
            if (timeLeft >= basicTime - 10) return true;
            return SceneManager.GetActiveScene().name == currentStageScene ? false : true;
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
