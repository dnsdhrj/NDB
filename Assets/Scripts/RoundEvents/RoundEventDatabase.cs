using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RoundEventDatabase : MonoBehaviour {
    public List<RoundEvent> roundEventDatabase = new List<RoundEvent>();

    private GameManager gm;

    //라운드행사 데이터베이스는 여기에 추가
    void Start()
    {
        gm = GameManager.gm;
        
        //행사 명칭, 함수 명, ID, 가격, 필요 개발자 수, 설명
        roundEventDatabase.Add(new RoundEvent("경험치 두배 이벤트", "ExpEvent", 0, 2000, 3, "인기도 상승율이 증가합니다.\n 중수, 고수 유저가 빠르게 증가합니다.", ExpEvent));
        roundEventDatabase.Add(new RoundEvent("캐시아이템 할인 이벤트", "CashItemDiscount", 1, 1000, 3, "수익이 감소합니다.\n 초보 유저 유입이 증가합니다.", CashItemDiscount));
        roundEventDatabase.Add(new RoundEvent("레어아이템 드랍율 상승 이벤트", "ItemDropIncrease", 2, 1000, 3, "초보 유저 유입이 감소합니다.\n 중수, 고수 유저가 빠르게 증가합니다.", ItemDropIncrease));
        roundEventDatabase.Add(new RoundEvent("명절 이벤트", "ItemDropIncrease", 3, 3000, 10, "초보 유저 유입이 증가합니다.\n 인기도 상승율이 증가합니다.", Holiday));

        /*roundEventDatabase.Add(new RoundEvent("약속과 믿음", "Kiri", 1, 777, 5, "유저 수 증가율이 감소합니다.",
            delegate() { gm.UserChange += Kiri; } ));*/
        //roundEventDatabase.Add(new RoundEvent("헤파이스토스", 1, 888, "유저가 감소합니다."));
    }


    //itemDatabase와는 달리 다른 script(e.g. inventory)가 없어 이곳에 함수를 넣음. 나중에 다른 적절한 script가 생기면 옮기자.
    public void AddRoundEvent(RoundEvent roundEvent)
    {
        if (GameManager.gm.isRoundEventOn)
        {
            GameManager.gm.ShowMessageBox("이미 적용된 행사가 있습니다.");
            return;
        }

        else
        {
            if (GameManager.gm.Money() < roundEvent.eventPrice)
            {
                GameManager.gm.ShowMessageBox("돈이 부족합니다.");
                return;
            }

            else if (Developer.dev.developerCount[Developer.dev.FindPostIDByName("Publicity")] < roundEvent.eventRequiredDev) {
                GameManager.gm.ShowMessageBox("개발자 수가 부족합니다.");
            }

            else
            {
                GameManager.gm.ChangeMoneyInterRound(-roundEvent.eventPrice);
                GameManager.gm.isRoundEventOn = true;

                GameManager.gm.ShowMessageBox("행사가 적용되었습니다.");
                StartCoroutine(roundEvent.eventStart());
            }
        }
    }

    IEnumerator ExpEvent()
    {
        float userUpdateTime = Time.time;

        while (true)
        {
            if (GameManager.gm.isRoundEventOn == false)
                StopCoroutine(ExpEvent());

            gm.FameDaram1();
            if (Unlockables.GetBool("UnlockDaram1") == true)
            {
                gm.FameDaram2();
                if (GiveDelay(1.0f, ref userUpdateTime))
                    gm.UserLevel2();
            }


            yield return StartCoroutine(WaitFor.Frames(3)); // 3프레임 당 한번 = 33.3% 
        }
    }

    private int PrevUser = 0;
    void Kiri()
    {
        Debug.Log("Kiri");
        if (GameManager.gm.isRoundEventOn == false)
            GameManager.gm.UserChange -= Kiri;

        //일단 중급 유저 증가량만 감소
        if (PrevUser == 0)
            PrevUser = gm.userCount[User.level2];
        else
        {
            int DeltaUser = gm.userCount[User.level2] - PrevUser;
            if(DeltaUser > 0)
                gm.userCount[User.level2] -= DeltaUser / 2;

            PrevUser = gm.userCount[User.level2];
        }
    } // 현재 미사용


    IEnumerator CashItemDiscount()
    {
        float userUpdateTime = Time.time;
        float moneyUpdateTime = Time.time;

        while (true)
        {
            if (GameManager.gm.isRoundEventOn == false)
                StopCoroutine(CashItemDiscount());

            if (GiveDelay(1.0f, ref userUpdateTime))
                gm.UserLevel1();

            if(GiveDelay(gm.timePerEarnedMoney, ref moneyUpdateTime))
                gm.ChangeMoneyInRound(gm.CalculateMoney(0.5f));
            yield return StartCoroutine(WaitFor.Frames(2)); // 2프레임 당 한번 = 50%
        }
    }

    IEnumerator ItemDropIncrease()
    {
        float userLevel1Decrease = 0.4f; // 초보유저 감소 비율
        float userUpdateTime = Time.time;

        while (true)
        {
            if (gm.isRoundEventOn == false)
                StopCoroutine(ItemDropIncrease());

            if (GiveDelay(1.0f, ref userUpdateTime))
            {
                gm.userCount[User.level1] -= (int)(userLevel1Decrease * gm.userLevel1Increase);
                if (Unlockables.GetBool("UnlockDaram1") == true)
                    gm.UserLevel2();
            }

            yield return StartCoroutine(WaitFor.Frames(2)); // 2프레임 당 한번 = 50%;
        }
    }

    IEnumerator Holiday()
    {
        float userUpdateTime = Time.time;
        while (true)
        {
            if (gm.isRoundEventOn == false)
                StopCoroutine(Holiday());

            if (GiveDelay(1.0f, ref userUpdateTime))
                gm.UserLevel1();

            gm.FameDaram1();
            if (Unlockables.GetBool("UnlockDaram1") == true)
                gm.FameDaram2();

            yield return StartCoroutine(WaitFor.Frames(1)); // 1프레임 당 한번 = 100%
        }
    }

    bool GiveDelay(float delay, ref float prevTime) {
        if (Time.time - prevTime > delay)
        {
            prevTime = Time.time;
            return true;
        }
        else return false;
    }
}


public static class WaitFor
{
    public static IEnumerator Frames(int frameCount)
    {

        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}

