using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour {

    private Text Timetext;

	void Start () {
        Timetext = GetComponent<Text>();
        StartCoroutine(ShowTime());
    }
	
	// Update is called once per frame
	void Update () {

    }

    private IEnumerator ShowTime() {

        int TimeLeft = GameManager.gm.timeLeft;
        int min = TimeLeft / 60;
        int sec = Mathf.Max(0, (int)(TimeLeft % 60));
        Timetext.text = min + " : " + sec;
        if(!(GameManager.gm.isPaused))
            GameManager.gm.timeLeft = --TimeLeft;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ShowTime());
    }
}
