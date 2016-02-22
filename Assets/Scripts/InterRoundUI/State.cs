using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class State : MonoBehaviour {

    private Text moneyText;
    private Text developerText;

	// Use this for initialization
	void Start () {
        moneyText = GameObject.Find("MoneyState").GetComponent<Text>();
        developerText = GameObject.Find("DeveloperState").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        moneyText.text = "" + GameManager.gm.money;
        developerText.text = "" + Developer.dev.salaryCost;
	}
}
