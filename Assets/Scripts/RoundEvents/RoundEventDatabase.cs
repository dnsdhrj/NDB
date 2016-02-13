using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoundEventDatabase : MonoBehaviour {
    public List<RoundEvent> roundEventDatabase = new List<RoundEvent>();

    //아이템 데이터베이스는 여기에 추가
    void Start()
    {
        roundEventDatabase.Add(new RoundEvent("경험치 두배 이벤트", 0, 1000, 3, "인기도 상승율이 증가합니다.\n 유저 수준이 올라갑니다."));
        roundEventDatabase.Add(new RoundEvent("약속과 믿음", 1, 777, 5, "유저 수 증가율이 감소합니다."));
        //roundEventDatabase.Add(new RoundEvent("헤파이스토스", 1, 888, "유저가 감소합니다."));
    }


    //아이템 효과는 여기에 추가
    public void applyEvent(RoundEvent rEvent)
    {
        switch (rEvent.eventID)
        {
            default:
                break;
        }
    }
}
