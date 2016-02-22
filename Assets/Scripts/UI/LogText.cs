using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogText : MonoBehaviour {
    
    private Text logText;


    public GameObject scrollbar;
    private Scrollbar logScrollbar;
    public string initialText = "";
    private static LogText _this;

    // Use this for initialization
    void Awake () 
    {
        _this = this;

        logText = GetComponent<Text>();
        logText.text = initialText;

        logScrollbar = scrollbar.GetComponent<Scrollbar>();
        logScrollbar.size = 0;
    }

    public static void WriteLog(string log)
    {
        _this.logText.text += "\n" + log;
        _this.logScrollbar.value = 0;
    }

    private bool StartText = false;
    void Update()
    {
        if (GameManager.gm.isInterRound == false && StartText == false)
        {
            WriteLog("\n로그인 시작!");
            StartText = true;
        }
    }
}
