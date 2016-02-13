using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RoundEventDatabase : MonoBehaviour {
    public List<RoundEvent> roundEventDatabase = new List<RoundEvent>();

    //라운드이벤트 데이터베이스는 여기에 추가
    void Start()
    {
        roundEventDatabase.Add(new RoundEvent("경험치 두배 이벤트", "ExpEvent", 0, 1000, 3, "인기도 상승율이 증가합니다.\n 유저 수준이 올라갑니다."));
        roundEventDatabase.Add(new RoundEvent("약속과 믿음", "Kiri", 1, 777, 5, "유저 수 증가율이 감소합니다."));
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

    //이렇게 안해도 되는 좋은 방법을 찾습니다
    void FindRoundEvent(int id)
    {
        switch (id)
        {
            case 0:
                GameManager.gm.EventCheck += ExpEvent;
                break;

            case 1:
                GameManager.gm.EventCheck += Kiri;
                break;

            default:
                break;
        }
    }

    void ExpEvent()
    {
        Debug.Log("ExpEvent");
        if (GameManager.gm.isRoundEventOn == false)
            GameManager.gm.EventCheck -= ExpEvent;
    }

    void Kiri()
    {
        Debug.Log("Kiri");
        if (GameManager.gm.isRoundEventOn == false)
            GameManager.gm.EventCheck -= Kiri;
    }
}
