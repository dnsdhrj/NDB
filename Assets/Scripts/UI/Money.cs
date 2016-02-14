using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Money : MonoBehaviour {

    private Text MoneyText;

    private int InitialMoney; // 스테이지 시작 시의 돈 (스테이지 진행 중의 소모는 모두 여기서 이루어 짐)
    private int EarnedMoney; // 스테이지 진행과정에서 버는 돈 (스테이지가 끝나야 다음 스테이지의 Initial Money에 추가 됨)

    void Start () {
        MoneyText = GetComponent<Text>();
	}
	
	void Update () {
        ShowMeTheMoney();
        InitialMoney = GameManager.gm.money;
        EarnedMoney = GameManager.gm.earnedMoney;
    }

    private void ShowMeTheMoney()
    {
        /*실제*/ //MoneyText.text = "Money : " + InitialMoney;
               /*Debug용*/
        MoneyText.text = "Money : " + InitialMoney + "\n" + "(Debug)\nEarnedMoney : " + EarnedMoney;
    }
}
