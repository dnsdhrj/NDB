using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoundEventDatabase : MonoBehaviour {
    public List<RoundEvent> roundEventDatabase = new List<RoundEvent>();

    //아이템 데이터베이스는 여기에 추가
    void Start()
    {
        roundEventDatabase.Add(new RoundEvent("ExpEvent", 0, 1000, "인기도 상승율이 증가합니다.\n 유저 수준이 올라갑니다."));
        roundEventDatabase.Add(new RoundEvent("PromiseAndBelief", 1, 777, "유저 수 증가율이 감소합니다."));
        roundEventDatabase.Add(new RoundEvent("Hephaistos", 1, 888, "게임이 망합니다."));
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
