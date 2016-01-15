﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour {

    private int TimeLeft;
    private Text Timetext;

	void Start () {
        GameManager.gm.SetStageTime();
        TimeLeft = GameManager.gm.TimeLeft;
        Timetext = GetComponent<Text>();
        StartCoroutine(ShowTime());
    }
	
	// Update is called once per frame
	void Update () {

    }

    private IEnumerator ShowTime() {
        int min = TimeLeft / 60;
        int sec = (int)(TimeLeft % 60);
        Timetext.text = min + " : " + sec;
        TimeLeft--;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ShowTime());
    }
}