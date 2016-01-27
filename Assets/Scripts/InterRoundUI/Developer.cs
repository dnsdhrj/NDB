using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Developer : MonoBehaviour {

    private Text hireButtonText;
    private Text fireButtonText;
    private int hireCost;
    private int fireCost;

	// Use this for initialization
	void Start () {
        hireButtonText = GameObject.Find("HireButton").GetComponentInChildren<Text>();
        fireButtonText = GameObject.Find("FireButton").GetComponentInChildren<Text>();
        CalculateCost();

	}

    void CalculateCost() {
        //누군가 적절한 함수를 생각해주길
        hireCost = GameManager.gm.Developers * 1000 + 1000;
        fireCost = GameManager.gm.Developers * 300 + 300;
        hireButtonText.text = "고용\n(" + hireCost + ")";
        fireButtonText.text = "해고\n(" + fireCost + ")";
    }

    public void HireDeveloper() {
        if (GameManager.gm.Money >= hireCost) {
            GameManager.gm.Money -= hireCost;
            GameManager.gm.Developers++;
            CalculateCost();
        }
    }

    public void FireDeveloper() {
        if (GameManager.gm.Money >= fireCost && GameManager.gm.Developers > 0) {
            GameManager.gm.Money -= fireCost;
            GameManager.gm.Developers--;
            CalculateCost();
        }
    }
}
