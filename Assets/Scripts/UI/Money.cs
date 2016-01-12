using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Money : MonoBehaviour {

    private Text MoneyText;

    private int InitialMoney; // 스테이지 시작 시의 돈 (스테이지 진행 중의 소모는 모두 여기서 이루어 짐)
    private int EarnedMoney; // 스테이지 진행과정에서 버는 돈 (스테이지가 끝나야 다음 스테이지의 Initial Money에 추가 됨)

    void Start () {
        MoneyText = GetComponent<Text>();
        InitialMoney = GameManager.gm.Money;
        EarnedMoney = 0;
	}
	
	void Update () {
        EarnedMoney += MoneyGainByFame();
        ShowMeTheMoney();
	}

    //인기도에 의해 정기적으로 버는 소득
    private int MoneyGainByFame() { 
        int RegularGain = 0;

        // 인기도에 비례한 적절한 함수
        RegularGain += GameManager.gm.Fame / 100;
        //////////////////////////////

        return RegularGain;
    }

    //현재 남은 돈
    private void ShowMeTheMoney() {
        /*실제*/ //MoneyText.text = "Money : " + InitialMoney;
        /*Debug용*/ MoneyText.text = "Money : " + InitialMoney + "\n" + "(DeBug) EarnedMoney : " + EarnedMoney;
    }

    //다람쥐 생성에서 소모되는 비용
    public void DaramLoss(int DaramCost) {
        InitialMoney -= DaramCost;
    }

    //이벤트(홍보, 긴급점검 등)에 의해 발생하는 돈의 증감
    //둘 다 + 이므로 parameter에 양수/음수를 잘 선정해서 넣어줘야 함
    public void MoneyGainByEvent(int EventGain, int EventCost) {
        EarnedMoney += EventGain;
        InitialMoney += EventCost;
    }

    //스테이지가 끝날 때 GameManager에 결과 저장
    public void MoneyUpdate() {
        GameManager.gm.Money = (InitialMoney + EarnedMoney);
    }
}
