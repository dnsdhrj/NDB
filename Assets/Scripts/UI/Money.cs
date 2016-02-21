using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Money : MonoBehaviour {

    private Text MoneyText;

    private int updatedMoney; // 스테이지 진행 중의 돈

    void Start () {
        MoneyText = GetComponent<Text>();
	}
	
	void Update () {
        ShowMeTheMoney();
        updatedMoney = GameManager.gm.Money();
    }

    private void ShowMeTheMoney()
    {
        /*실제*/ //MoneyText.text = "Money : " + InitialMoney;
               /*Debug용*/
        MoneyText.text = "보유한 돈\n\t" + updatedMoney;
    }
}
