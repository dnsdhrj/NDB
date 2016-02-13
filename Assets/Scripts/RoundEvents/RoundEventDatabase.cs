using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RoundEventDatabase : MonoBehaviour {
    public List<RoundEvent> roundEventDatabase = new List<RoundEvent>();

    private GameManager gm;

    //라운드이벤트 데이터베이스는 여기에 추가
    void Start()
    {
        gm = GameManager.gm;

        roundEventDatabase.Add(new RoundEvent("경험치 두배 이벤트", "ExpEvent", 0, 1000, 3, "인기도 상승율이 증가합니다.\n 유저 수준이 올라갑니다.",
            delegate() { gm.FameChange += ExpEvent; gm.UserChange += ExpEvent2; } ));
        roundEventDatabase.Add(new RoundEvent("약속과 믿음", "Kiri", 1, 777, 5, "유저 수 증가율이 감소합니다.",
            delegate() { gm.UserChange += Kiri; } ));
        //roundEventDatabase.Add(new RoundEvent("헤파이스토스", 1, 888, "유저가 감소합니다."));
    }


    //itemDatabase와는 달리 다른 script(e.g. inventory)가 없어 이곳에 함수를 넣음 나중에 다른 적절한 script가 생기면 옴기자.
    public void AddRoundEvent(RoundEvent roundEvent)
    {
        if (GameManager.gm.isRoundEventOn)
        {
            //나중에 창으로 나오게 고치자
            Debug.Log("이미 적용된 라운드 이벤트가 있습니다.");
            return;
        }

        else
        {
            if (GameManager.gm.Money < roundEvent.eventPrice)
            {
                //나중에 창으로 나오게 고치자
                Debug.Log("돈이 부족합니다.");
                return;
            }

            else
            {
                GameManager.gm.Money -= roundEvent.eventPrice;
                GameManager.gm.isRoundEventOn = true;
                FindRoundEvent(roundEvent.eventID);
            }
        }
    }

    //이렇게 안해도 되는 좋은 방법을 찾습니다 -> 수정은 했는데 좋은 방법인지는 잘 모르겠음
    void FindRoundEvent(int id)
    {
        /*switch (id)
        {
            case 0:
                GameManager.gm.FameChange += ExpEvent;
                GameManager.gm.UserChange += ExpEvent2;
                break;

            case 1:
                GameManager.gm.UserChange += Kiri;
                break;

            default:
                break;
        }*/
        roundEventDatabase[id].eventStart();
    }

    void ExpEvent()
    {
        Debug.Log("ExpEvent");
        if (GameManager.gm.isRoundEventOn == false)
            GameManager.gm.FameChange -= ExpEvent;

        //인기도 30% 증가
        gm.Fame += (int) (gm.DaramFunction[User.level1].value / 3.0f);
        if(Unlockables.GetBool("UnlockDaram1") == true)
            gm.Fame += (int)(gm.DaramFunction[User.level2].value / 3.0f);
    }

    void ExpEvent2()
    {
        if (GameManager.gm.isRoundEventOn == false)
            GameManager.gm.UserChange -= ExpEvent2;

        //경험치가 두배니까 유저도 두배
        gm.UserLevel2();
    }

    private int PrevUser = 0;
    void Kiri()
    {
        Debug.Log("Kiri");
        if (GameManager.gm.isRoundEventOn == false)
            GameManager.gm.UserChange -= Kiri;

        //일단 중급 유저 증가량만 감소
        if (PrevUser == 0)
            PrevUser = gm.UserCount[User.level2];
        else
        {
            int DeltaUser = gm.UserCount[User.level2] - PrevUser;
            if(DeltaUser > 0)
                gm.UserCount[User.level2] -= DeltaUser / 2;

            PrevUser = gm.UserCount[User.level2];
        }
    }
}
