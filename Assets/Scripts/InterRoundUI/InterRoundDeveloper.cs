using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterRoundDeveloper : MonoBehaviour {

    private Text hireButtonText;
    private Text fireButtonText;

	// Use this for initialization
	void Start () {
        hireButtonText = GameObject.Find("HireButton").GetComponentInChildren<Text>();
        fireButtonText = GameObject.Find("FireButton").GetComponentInChildren<Text>();
        hireButtonText.text = "고용\n(" + Developer.dev.hireCost + ")";
        fireButtonText.text = "해고\n(" + Developer.dev.fireCost + ")";
	}

    // InterRound에서 바로 Developer 클래스의 함수에 접근할 수 없어서 만들었음
    // post, from, to는 각 부서명에 해당하는 int값이고 이는 Developer.cs 파일에 있는 주석 참조.

    public void Hire(int post)
    {
        Developer.dev.HireDeveloper(post);
    }

    public void Fire(int post)
    {
        Developer.dev.FireDeveloper(post);
    }

    public void Move(int from, int to)
    {
        Developer.dev.MoveDeveloper(from, to);
    }
}
