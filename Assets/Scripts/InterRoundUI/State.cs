using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class State : MonoBehaviour {

    private Text moneyText;
    private Text developerText;
    private Text itemText;

	// Use this for initialization
	void Start () {
        moneyText = GameObject.Find("MoneyState").GetComponent<Text>();
        developerText = GameObject.Find("DeveloperState").GetComponent<Text>();
        itemText = GameObject.Find("ItemState").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        moneyText.text = "" + GameManager.gm.money;
        developerText.text = "" + Developer.dev.DeveloperAllCount();
        //itemText = 아이템 구현하고 나서 추가합시당
	}
}
