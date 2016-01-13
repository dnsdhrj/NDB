using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogText : MonoBehaviour {
    
    private Text logText;


    public GameObject scrollbar;
    private Scrollbar logScrollbar;
    public string initialText = "드디어 새 게임을 출시했다! 다람쥐를 뿌려 인기도를 높여보자.";
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
}
