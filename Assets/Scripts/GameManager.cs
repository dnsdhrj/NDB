﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    [HideInInspector] public int Money = 0;
    [HideInInspector] public int EarnedMoney = 0;
    [HideInInspector] public int Fame = 0;
    [HideInInspector] public int[] UserCount;
    [HideInInspector] public int StageLevel = 1;
    [HideInInspector] public int TimeLeft;
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

    // public 함수들
    //public Vector2 RandomPosition();


    void Awake()
    {
        gm = this;

        //UserCount 모두 0으로 초기화
        UserCount = Enumerable.Repeat(0, User.Count).ToArray();

        //테스트용이고 나중에 삭제바람
        DaramDeath += DaramDeath_test;
        FameChange += FameChange_test;
        UserChange += UserChange_test;

        FameChange += CheckFameZero;
        

        Random.seed = (int)Time.time;
    }

    void Start()
    {
        StartCoroutine("UserChangeCall");
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

        if (Input.GetKeyDown("f2")) //디버그용
            DebugFunc();
        if (Input.GetKeyDown("f3")) //각종 변수 상태 출력
            DebugStatFunc();

        EarnedMoney += MoneyGainByFame();
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

    void DaramDeath_test()
    {
        int count = 0;

        // 어떤 요인에 의해
        if (Daram.All.Count != 0)
            count = (int)(UserCount[User.level1] / 1000.0f + UserCount[User.level2] / 100.0f + 1);

        //다람쥐에게 피해를 입힌다
        for (int i = 0; i < count; i++)
        {
            int all = Daram.All.Count;
            if (all != 0)
                Daram.All[Random.Range(0, all)].HP -= 1;
        }
    }

    void FameChange_test()
    {
        //다람쥐의 적정 숫자
        int targetnumber = 10 + Fame / 1000;

        // f(x) = 5 - | y - x |
        Fame += 5 - Mathf.Abs(Daram.All.Count - targetnumber);
    }

    void UserChange_test()
    {
            UserCount[User.level1] = Mathf.Min(Fame / 2, UserCount[User.level1] + Fame / 50);
            UserCount[User.level2] = Mathf.Min(Fame / 10, UserCount[User.level2] + Fame / 500);
    }

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

    /* Functions about Money */

    //인기도에 의해 정기적으로 버는 소득
    int MoneyGainByFame()
    {
        int RegularGain = 0;

        // 인기도에 비례한 적절한 함수
        RegularGain += Fame / 100;
        //////////////////////////////

        return RegularGain;
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
        int BasicTime = 60;
        TimeLeft = BasicTime + StageLevel * 10;
    }

    void StageEndCheck() {
        if (TimeLeft <= 0) {
            //Stage End
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
